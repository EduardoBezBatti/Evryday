namespace Habitos.Dominio;

public record Sequencia(int Atual, int Maior);

public static class CalculadoraDeSequencia
{
    public static Sequencia Calcular(Habito habito, DateOnly hoje)
    {
        return new Sequencia(
            CalcularAtual(habito, hoje),
            CalcularMaior(habito, hoje)
        );
    }

    private static int CalcularAtual(Habito habito, DateOnly hoje)
    {
        int streak = 0;
        DateOnly data = hoje;

        // Se hoje é previsto e NÃO está marcado, a sequência atual já é 0
        if (habito.PreveDia(data) && !habito.EstaMarcadoEm(data))
        {
            return 0;
        }

        // Retrocedemos no tempo
        while (true)
        {
            data = data.AddDays(-1);
            
            // Se chegarmos na data de início do hábito, paramos
            if (data < habito.DataInicio) break;

            if (habito.PreveDia(data))
            {
                if (habito.EstaMarcadoEm(data))
                {
                    streak++;
                }
                else
                {
                    // Dia previsto não marcado quebra a sequência
                    break;
                }
            }
            // Dias não previstos são ignorados e não quebram a sequência
        }

        // Adicionamos o dia de hoje se ele for previsto e estiver marcado
        if (habito.PreveDia(hoje) && habito.EstaMarcadoEm(hoje))
        {
            streak++;
        }

        return streak;
    }

    private static int CalcularMaior(Habito habito, DateOnly hoje)
    {
        int maiorStreak = 0;
        int streakAtual = 0;
        DateOnly data = habito.DataInicio;

        while (data <= hoje)
        {
            if (habito.PreveDia(data))
            {
                if (habito.EstaMarcadoEm(data))
                {
                    streakAtual++;
                }
                else
                {
                    maiorStreak = Math.Max(maiorStreak, streakAtual);
                    streakAtual = 0;
                }
            }
            data = data.AddDays(1);
        }

        return Math.Max(maiorStreak, streakAtual);
    }
}
