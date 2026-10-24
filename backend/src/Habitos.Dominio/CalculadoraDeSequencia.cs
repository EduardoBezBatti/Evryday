namespace Habitos.Dominio;

public record Sequencia(int Atual, int Maior);

public static class CalculadoraDeSequencia
{
    public static Sequencia Calcular(Habito habito, DateOnly hoje)
    {
        var freq = habito.FrequenciaEm(hoje);

        if (freq.Tipo == TipoFrequencia.VezesPorSemana)
        {
            return CalcularSemanal(habito, hoje, freq.VezesPorSemana ?? 1);
        }

        return new Sequencia(
            CalcularAtualDiario(habito, hoje),
            CalcularMaiorDiario(habito, hoje)
        );
    }

    private static Sequencia CalcularSemanal(Habito habito, DateOnly hoje, int meta)
    {
        int streakSemanas = 0;
        
        var inicioSemanaAtual = Semana.InicioDe(hoje);
        var fimSemanaAtual = inicioSemanaAtual.AddDays(6);
        
        int marcadosHoje = habito.MarcacoesNaSemana(inicioSemanaAtual);
        int diasPossiveisHoje = fimSemanaAtual.DayNumber - hoje.DayNumber + 1;

        if (marcadosHoje + diasPossiveisHoje < meta)
        {
            return new Sequencia(0, 0);
        }

        // Retrocedemos a partir da semana anterior à atual
        DateOnly dataReferencia = inicioSemanaAtual.AddDays(-7);

        while (true)
        {
            if (dataReferencia < habito.DataInicio) break;

            // Valida a semana que começa em dataReferencia (que já é um início de semana)
            if (habito.MarcacoesNaSemana(dataReferencia) >= meta)
            {
                streakSemanas++;
            }
            else
            {
                break; 
            }

            dataReferencia = dataReferencia.AddDays(-7);
        }

        return new Sequencia(streakSemanas, streakSemanas);
    }

    private static int CalcularAtualDiario(Habito habito, DateOnly hoje)
    {
        int streak = 0;
        DateOnly data = hoje;

        if (habito.PreveDia(data) && !habito.EstaMarcadoEm(data))
        {
            return 0;
        }

        while (true)
        {
            data = data.AddDays(-1);
            if (data < habito.DataInicio) break;

            if (habito.PreveDia(data))
            {
                if (habito.EstaMarcadoEm(data))
                {
                    streak++;
                }
                else
                {
                    break;
                }
            }
        }

        if (habito.PreveDia(hoje) && habito.EstaMarcadoEm(hoje))
        {
            streak++;
        }

        return streak;
    }

    private static int CalcularMaiorDiario(Habito habito, DateOnly hoje)
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
