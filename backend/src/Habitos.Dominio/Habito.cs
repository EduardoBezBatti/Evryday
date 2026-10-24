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

    public Frequencia FrequenciaEm(DateOnly data)
    {
        var agenda = _agendas
            .Where(a => a.VigenteDesde <= data)
            .OrderByDescending(a => a.VigenteDesde)
            .FirstOrDefault();

        if (agenda != null) return agenda.Frequencia;

        var primeiraAgenda = _agendas
            .OrderBy(a => a.VigenteDesde)
            .FirstOrDefault();

        return primeiraAgenda?.Frequencia ?? Frequencia.Diaria();
    }

    public bool PreveDia(DateOnly data)
    {
        return FrequenciaEm(data).PreveDia(data);
    }

    public bool AlterarFrequencia(Frequencia novaFrequencia, DateOnly hoje)
    {
        if (novaFrequencia is null)
        {
            throw new ExcecaoDeValidacao("frequencia", "A frequência é obrigatória.");
        }

        // 1. Se FrequenciaEm(hoje) == nova, não faz nada
        if (FrequenciaEm(hoje) == novaFrequencia)
        {
            return false;
        }

        // 2. Se já existe agenda com VigenteDesde == hoje
        var agendaDeHoje = _agendas.FirstOrDefault(a => a.VigenteDesde == hoje);
        if (agendaDeHoje is not null)
        {
            var agendaAnterior = _agendas
                .Where(a => a.VigenteDesde < hoje)
                .OrderByDescending(a => a.VigenteDesde)
                .FirstOrDefault();

            if (agendaAnterior is not null && agendaAnterior.Frequencia == novaFrequencia)
            {
                // Se ficou igual à agenda anterior, remove a agenda de hoje
                _agendas.Remove(agendaDeHoje);
                return true;
            }
            else
            {
                // Substitui a frequência da agenda de hoje
                agendaDeHoje.AtualizarFrequencia(novaFrequencia);
                return true;
            }
        }

        // 3. Senão, adiciona uma nova agenda com VigenteDesde = hoje
        var novaAgenda = new AgendaHabito(Guid.CreateVersion7(), Id, hoje, novaFrequencia);
        _agendas.Add(novaAgenda);
        return true;
    }

    public bool EstaMarcadoEm(DateOnly data)
    {
        return _marcacoes.Any(m => m.Data == data);
    }

    public bool EhExtra(DateOnly data)
    {
        return EstaMarcadoEm(data) && !PreveDia(data);
    }

    public void Marcar(DateOnly data, DateOnly hoje, DateTime agoraUtc)
    {
        if (data != hoje)
        {
            throw new ExcecaoDeRegraDeNegocio("Só é possível marcar o dia de hoje");
        }

        if (EstaMarcadoEm(data))
        {
            return;
        }

        var marcacao = new Marcacao(Guid.CreateVersion7(), Id, data, agoraUtc);
        _marcacoes.Add(marcacao);
    }

    public void Desmarcar(DateOnly data, DateOnly hoje)
    {
        if (data != hoje)
        {
            throw new ExcecaoDeRegraDeNegocio("Só é possível marcar o dia de hoje");
        }

        var marcacao = _marcacoes.FirstOrDefault(m => m.Data == data);
        if (marcacao is not null)
        {
            _marcacoes.Remove(marcacao);
        }
    }

    public void AdicionarMarcacaoParaTeste(DateOnly data, DateTime agoraUtc)
    {
        if (EstaMarcadoEm(data)) return;
        _marcacoes.Add(new Marcacao(Guid.CreateVersion7(), Id, data, agoraUtc));
    }

    public int MarcacoesNaSemana(DateOnly data)

    {
        var inicioSemana = Semana.InicioDe(data);
        var fimSemana = inicioSemana.AddDays(6);

        return _marcacoes.Count(m => m.Data >= inicioSemana && m.Data <= fimSemana);
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
            throw new ExcecaoDeValidacao("cor", "A cor selecionada é inválida. Por favor, use um código hexadecimal (ex: #FF5252) ou um nome de cor aceito (ex: vermelho, azul, amarelo, verde, ciano, roxo, laranja, marrom).");
        }

        return (nomeFormatado, descricaoFormatada, PaletaDeCores.Traduzir(corFormatada).ToUpperInvariant());
    }
}
