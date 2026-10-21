namespace Habitos.Dominio.Testes;

public class MarcacaoTestes
{
    private readonly DateOnly _hoje = new(2026, 10, 20); // Terça-feira
    private readonly DateTime _agoraUtc = new(2026, 10, 20, 14, 0, 0, DateTimeKind.Utc);
    private const string CorValida = "#43A047";

    [Fact]
    public void Marcar_DataIgualAHoje_RegistraMarcacaoComSucesso()
    {
        // 001 CA 6.1: registra marcação na data de hoje
        var habito = Habito.Criar("Correr", null, CorValida, Frequencia.Diaria(), _hoje, _agoraUtc);

        habito.Marcar(_hoje, _hoje, _agoraUtc);

        Assert.True(habito.EstaMarcadoEm(_hoje));
        Assert.Single(habito.Marcacoes);
        var marcacao = habito.Marcacoes.First();
        Assert.Equal(_hoje, marcacao.Data);
        Assert.Equal(_agoraUtc, marcacao.CriadaEmUtc);
        Assert.Equal(habito.Id, marcacao.HabitoId);
    }

    [Theory]
    [InlineData(-1)] // Ontem
    [InlineData(1)]  // Amanhã
    public void Marcar_DataDiferenteDeHoje_LancaExcecaoDeRegraDeNegocio(int diasDiferenca)
    {
        // 001 CA 6.4: SE for pedida a marcação de uma data diferente de hoje, ENTÃO o sistema DEVE recusar
        var habito = Habito.Criar("Correr", null, CorValida, Frequencia.Diaria(), _hoje, _agoraUtc);
        var dataInvalida = _hoje.AddDays(diasDiferenca);

        var excecao = Assert.Throws<ExcecaoDeRegraDeNegocio>(() =>
            habito.Marcar(dataInvalida, _hoje, _agoraUtc));

        Assert.Equal("Só é possível marcar o dia de hoje", excecao.Message);
        Assert.False(habito.EstaMarcadoEm(dataInvalida));
    }

    [Fact]
    public void Marcar_HabitoJaMarcadoHoje_IdempotenteNaoCriaOutraMarcacao()
    {
        // 001 CA 6.3: marcar um hábito já marcado NÃO DEVE criar outra marcação
        var habito = Habito.Criar("Correr", null, CorValida, Frequencia.Diaria(), _hoje, _agoraUtc);

        habito.Marcar(_hoje, _hoje, _agoraUtc);
        habito.Marcar(_hoje, _hoje, _agoraUtc.AddMinutes(5));

        Assert.Single(habito.Marcacoes);
    }

    [Fact]
    public void Desmarcar_HabitoMarcadoHoje_RemoveMarcacao()
    {
        // 001 CA 6.2: QUANDO o usuário desmarcar um hábito marcado hoje, o sistema DEVE remover a marcação de hoje
        var habito = Habito.Criar("Correr", null, CorValida, Frequencia.Diaria(), _hoje, _agoraUtc);
        habito.Marcar(_hoje, _hoje, _agoraUtc);

        habito.Desmarcar(_hoje, _hoje);

        Assert.False(habito.EstaMarcadoEm(_hoje));
        Assert.Empty(habito.Marcacoes);
    }

    [Fact]
    public void Desmarcar_HabitoNaoMarcado_NaoGeraErro()
    {
        // 001 CA 6.3: desmarcar um hábito não marcado NÃO DEVE gerar erro
        var habito = Habito.Criar("Correr", null, CorValida, Frequencia.Diaria(), _hoje, _agoraUtc);

        habito.Desmarcar(_hoje, _hoje);

        Assert.False(habito.EstaMarcadoEm(_hoje));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    public void Desmarcar_DataDiferenteDeHoje_LancaExcecaoDeRegraDeNegocio(int diasDiferenca)
    {
        // 001 CA 6.4: SE for pedida a desmarcação de uma data diferente de hoje, ENTÃO o sistema DEVE recusar
        var habito = Habito.Criar("Correr", null, CorValida, Frequencia.Diaria(), _hoje, _agoraUtc);
        var dataInvalida = _hoje.AddDays(diasDiferenca);

        var excecao = Assert.Throws<ExcecaoDeRegraDeNegocio>(() =>
            habito.Desmarcar(dataInvalida, _hoje));

        Assert.Equal("Só é possível marcar o dia de hoje", excecao.Message);
    }

    [Fact]
    public void EhExtra_HabitoDeDiasDaSemanaMarcadoEmDiaNaoPrevisto_RetornaVerdadeiro()
    {
        // 001 CA 6.5: QUANDO um hábito de dias da semana for marcado num dia não previsto, o sistema DEVE aceitar a marcação como extra
        // _hoje é terça-feira (2026-10-20). Frequência é Seg e Qua.
        var freqSegQua = Frequencia.NosDias(DiasDaSemana.Segunda | DiasDaSemana.Quarta);
        var habito = Habito.Criar("Academia", null, CorValida, freqSegQua, _hoje, _agoraUtc);

        habito.Marcar(_hoje, _hoje, _agoraUtc);

        Assert.True(habito.EstaMarcadoEm(_hoje));
        Assert.False(habito.PreveDia(_hoje));
        Assert.True(habito.EhExtra(_hoje));
    }

    [Fact]
    public void EhExtra_AposMudancaDeFrequencia_AtualizaClassificacaoAutomaticamente()
    {
        // 001 CA 3.5: QUANDO a frequência mudar e o hábito já estiver marcado hoje,
        // o sistema DEVE manter a marcação e classificá-la de acordo com a nova frequência (prevista ou extra)
        // Terça-feira (2026-10-20), freq Seg e Qua
        var freqSegQua = Frequencia.NosDias(DiasDaSemana.Segunda | DiasDaSemana.Quarta);
        var habito = Habito.Criar("Academia", null, CorValida, freqSegQua, _hoje, _agoraUtc);
        habito.Marcar(_hoje, _hoje, _agoraUtc);

        Assert.True(habito.EhExtra(_hoje));

        // Muda para diária na própria terça-feira
        habito.AlterarFrequencia(Frequencia.Diaria(), _hoje);

        Assert.True(habito.EstaMarcadoEm(_hoje));
        Assert.True(habito.PreveDia(_hoje));
        Assert.False(habito.EhExtra(_hoje));
    }

    [Fact]
    public void MarcacoesNaSemana_ContaApenasMarcacoesDeSegundaADomingoDaSemanaDaData()
    {
        // 001 CA 5.5: contando as marcações de segunda a domingo
        // Semana de 2026-10-19 (Seg) a 2026-10-25 (Dom)
        var inicioCriacao = new DateOnly(2026, 10, 10);
        var habito = Habito.Criar("Treino", null, CorValida, Frequencia.Diaria(), inicioCriacao, _agoraUtc);

        // Marca domingo anterior (2026-10-18): semana anterior
        habito.Marcar(new DateOnly(2026, 10, 18), new DateOnly(2026, 10, 18), _agoraUtc);

        // Marca nesta semana: segunda (19) e terça (20)
        habito.Marcar(new DateOnly(2026, 10, 19), new DateOnly(2026, 10, 19), _agoraUtc);
        habito.Marcar(new DateOnly(2026, 10, 20), new DateOnly(2026, 10, 20), _agoraUtc);

        // Marca semana seguinte: segunda (26)
        habito.Marcar(new DateOnly(2026, 10, 26), new DateOnly(2026, 10, 26), _agoraUtc);

        // Consulta a semana de 2026-10-20
        var contagem = habito.MarcacoesNaSemana(_hoje);

        Assert.Equal(2, contagem);
    }
}
