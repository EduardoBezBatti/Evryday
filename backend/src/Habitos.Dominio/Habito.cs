namespace Habitos.Dominio;

public class Habito
{
    private readonly List<AgendaHabito> _agendas = new();
    private readonly List<Marcacao> _marcacoes = new();

    public Guid Id { get; private init; }
    public string Nome { get; private set; }
    public string NomeNormalizado { get; private set; }
    public string? Descricao { get; private set; }
    public string Cor { get; private set; }
    public DateOnly DataInicio { get; private init; }
    public DateTime CriadoEmUtc { get; private init; }

    public IReadOnlyCollection<AgendaHabito> Agendas => _agendas.AsReadOnly();
    public IReadOnlyCollection<Marcacao> Marcacoes => _marcacoes.AsReadOnly();

    protected Habito()
    {
        Nome = string.Empty;
        NomeNormalizado = string.Empty;
        Cor = string.Empty;
    }

    private Habito(
        Guid id,
        string nome,
        string? descricao,
        string cor,
        DateOnly dataInicio,
        DateTime criadoEmUtc)
    {
        Id = id;
        Nome = nome;
        NomeNormalizado = nome.ToUpperInvariant();
        Descricao = descricao;
        Cor = cor;
        DataInicio = dataInicio;
        CriadoEmUtc = criadoEmUtc;
    }

    public static Habito Criar(
        string nome,
        string? descricao,
        string cor,
        Frequencia frequencia,
        DateOnly hoje,
        DateTime agoraUtc)
    {
        var (nomeValido, descricaoValida, corValida) = ValidarDados(nome, descricao, cor);

        if (frequencia is null)
        {
            throw new ExcecaoDeValidacao("frequencia", "A frequência é obrigatória.");
        }

        var id = Guid.CreateVersion7();
        var habito = new Habito(id, nomeValido, descricaoValida, corValida, hoje, agoraUtc);

        var primeiraAgenda = new AgendaHabito(Guid.CreateVersion7(), id, hoje, frequencia);
        habito._agendas.Add(primeiraAgenda);

        return habito;
    }

    public void AtualizarDados(string nome, string? descricao, string cor)
    {
        var (nomeValido, descricaoValida, corValida) = ValidarDados(nome, descricao, cor);

        Nome = nomeValido;
        NomeNormalizado = nomeValido.ToUpperInvariant();
        Descricao = descricaoValida;
        Cor = corValida;
    }

    private static (string Nome, string? Descricao, string Cor) ValidarDados(
        string? nome,
        string? descricao,
        string? cor)
    {
        var nomeFormatado = nome?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(nomeFormatado))
        {
            throw new ExcecaoDeValidacao("nome", "O nome do hábito é obrigatório.");
        }

        if (nomeFormatado.Length > 60)
        {
            throw new ExcecaoDeValidacao("nome", "O nome do hábito deve ter no máximo 60 caracteres.");
        }

        string? descricaoFormatada = null;
        if (!string.IsNullOrWhiteSpace(descricao))
        {
            var descTrim = descricao.Trim();
            if (descTrim.Length > 200)
            {
                throw new ExcecaoDeValidacao("descricao", "A descrição deve ter no máximo 200 caracteres.");
            }

            descricaoFormatada = descTrim;
        }

        var corFormatada = cor?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(corFormatada) || !PaletaDeCores.Contem(corFormatada))
        {
            throw new ExcecaoDeValidacao("cor", "A cor selecionada é inválida.");
        }

        return (nomeFormatado, descricaoFormatada, corFormatada.ToUpperInvariant());
    }
}
