namespace Habitos.Dominio;

public record Frequencia
{
    public TipoFrequencia Tipo { get; init; }
    public DiasDaSemana Dias { get; init; }
    public int? VezesPorSemana { get; init; }

    private Frequencia(TipoFrequencia tipo, DiasDaSemana dias, int? vezesPorSemana)
    {
        Tipo = tipo;
        Dias = dias;
        VezesPorSemana = vezesPorSemana;
    }

    protected Frequencia() { }

    public static Frequencia Diaria() =>
        new(TipoFrequencia.Diaria, DiasDaSemana.Nenhum, null);

    public static Frequencia NosDias(DiasDaSemana dias)
    {
        if (dias == DiasDaSemana.Nenhum)
        {
            throw new ExcecaoDeValidacao("dias", "Selecione ao menos um dia da semana.");
        }

        return new(TipoFrequencia.DiasDaSemana, dias, null);
    }

    public static Frequencia VezesNaSemana(int vezes)
    {
        if (vezes < 1 || vezes > 6)
        {
            throw new ExcecaoDeValidacao("vezesPorSemana", "A meta deve ser entre 1 e 6 vezes por semana.");
        }

        return new(TipoFrequencia.VezesPorSemana, DiasDaSemana.Nenhum, vezes);
    }

    public bool PreveDia(DateOnly data)
    {
        return Tipo switch
        {
            TipoFrequencia.Diaria => true,
            TipoFrequencia.VezesPorSemana => true,
            TipoFrequencia.DiasDaSemana => Dias.HasFlag(ConverterParaDiaDaSemana(data.DayOfWeek)),
            _ => false
        };
    }

    public static DiasDaSemana ConverterParaDiaDaSemana(DayOfWeek dayOfWeek) => dayOfWeek switch
    {
        DayOfWeek.Monday => DiasDaSemana.Segunda,
        DayOfWeek.Tuesday => DiasDaSemana.Terca,
        DayOfWeek.Wednesday => DiasDaSemana.Quarta,
        DayOfWeek.Thursday => DiasDaSemana.Quinta,
        DayOfWeek.Friday => DiasDaSemana.Sexta,
        DayOfWeek.Saturday => DiasDaSemana.Sabado,
        DayOfWeek.Sunday => DiasDaSemana.Domingo,
        _ => DiasDaSemana.Nenhum
    };
}
