namespace Habitos.Dominio;

public class AgendaHabito
{
    public Guid Id { get; private init; }
    public Guid HabitoId { get; private init; }
    public DateOnly VigenteDesde { get; private init; }
    public Frequencia Frequencia { get; private set; }

    internal AgendaHabito(Guid id, Guid habitoId, DateOnly vigenteDesde, Frequencia frequencia)
    {
        Id = id;
        HabitoId = habitoId;
        VigenteDesde = vigenteDesde;
        Frequencia = frequencia;
    }

    internal void AtualizarFrequencia(Frequencia novaFrequencia)
    {
        Frequencia = novaFrequencia;
    }

    protected AgendaHabito()
    {
        Frequencia = null!;
    }
}
