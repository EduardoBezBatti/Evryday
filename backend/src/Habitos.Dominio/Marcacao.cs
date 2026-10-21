namespace Habitos.Dominio;

public class Marcacao
{
    public Guid Id { get; private init; }
    public Guid HabitoId { get; private init; }
    public DateOnly Data { get; private init; }
    public DateTime CriadaEmUtc { get; private init; }

    internal Marcacao(Guid id, Guid habitoId, DateOnly data, DateTime criadaEmUtc)
    {
        Id = id;
        HabitoId = habitoId;
        Data = data;
        CriadaEmUtc = criadaEmUtc;
    }

    protected Marcacao() { }
}
