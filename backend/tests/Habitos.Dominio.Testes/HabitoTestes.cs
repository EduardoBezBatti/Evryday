namespace Habitos.Dominio.Testes;

public class HabitoTestes
{
    private readonly DateOnly _hoje = new(2026, 10, 20);
    private readonly DateTime _agoraUtc = new(2026, 10, 20, 18, 0, 0, DateTimeKind.Utc);
    private readonly Frequencia _frequenciaDiaria = Frequencia.Diaria();
    private const string CorValida = "#43A047";

    [Fact]
    public void Criar_DadosValidos_CriaHabitoComDataInicioEAprimeiraAgenda()
    {
        // 001 CA 1.1: cria o hábito com data de início igual a hoje e primeira agenda vigente desde hoje
        var habito = Habito.Criar("Correr", "Pela manhã", CorValida, _frequenciaDiaria, _hoje, _agoraUtc);

        Assert.NotEqual(Guid.Empty, habito.Id);
        Assert.Equal("Correr", habito.Nome);
        Assert.Equal("Pela manhã", habito.Descricao);
        Assert.Equal(CorValida, habito.Cor);
        Assert.Equal(_hoje, habito.DataInicio);
        Assert.Equal(_agoraUtc, habito.CriadoEmUtc);

        Assert.Single(habito.Agendas);
        var agenda = habito.Agendas.First();
        Assert.Equal(_hoje, agenda.VigenteDesde);
        Assert.Equal(_frequenciaDiaria, agenda.Frequencia);
    }

    [Fact]
    public void Criar_NomeComEspacosNasPontas_SalvaNomeAparado()
    {
        // 001 CA 1.2: desconsiderando espaços no início e no fim, e DEVE salvar o nome sem esses espaços
        var habito = Habito.Criar("   Meditar   ", null, CorValida, _frequenciaDiaria, _hoje, _agoraUtc);

        Assert.Equal("Meditar", habito.Nome);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Criar_NomeVazioOuEmBranco_LancaExcecaoDeValidacao(string nomeInvalido)
    {
        // 001 CA 1.2: exigir nome com 1 a 60 caracteres
        var excecao = Assert.Throws<ExcecaoDeValidacao>(() =>
            Habito.Criar(nomeInvalido, null, CorValida, _frequenciaDiaria, _hoje, _agoraUtc));

        Assert.Equal("nome", excecao.Campo);
    }

    [Fact]
    public void Criar_NomeComMaisDe60Caracteres_LancaExcecaoDeValidacao()
    {
        // 001 CA 1.2: exigir nome com 1 a 60 caracteres
        var nomeCom61 = new string('A', 61);

        var excecao = Assert.Throws<ExcecaoDeValidacao>(() =>
            Habito.Criar(nomeCom61, null, CorValida, _frequenciaDiaria, _hoje, _agoraUtc));

        Assert.Equal("nome", excecao.Campo);
    }

    [Fact]
    public void Criar_NomeComLimitesDe1e60Caracteres_CriaComSucesso()
    {
        var nomeCom1 = "A";
        var nomeCom60 = new string('B', 60);

        var habito1 = Habito.Criar(nomeCom1, null, CorValida, _frequenciaDiaria, _hoje, _agoraUtc);
        var habito60 = Habito.Criar(nomeCom60, null, CorValida, _frequenciaDiaria, _hoje, _agoraUtc);

        Assert.Equal(nomeCom1, habito1.Nome);
        Assert.Equal(nomeCom60, habito60.Nome);
    }

    [Fact]
    public void Criar_DescricaoComMaisDe200Caracteres_LancaExcecaoDeValidacao()
    {
        // 001 CA 1.4: aceitar uma descrição opcional de até 200 caracteres
        var descricaoCom201 = new string('D', 201);

        var excecao = Assert.Throws<ExcecaoDeValidacao>(() =>
            Habito.Criar("Leitura", descricaoCom201, CorValida, _frequenciaDiaria, _hoje, _agoraUtc));

        Assert.Equal("descricao", excecao.Campo);
    }

    [Fact]
    public void Criar_DescricaoOpcionalAte200CaracteresOuNula_CriaComSucesso()
    {
        var descricaoCom200 = new string('D', 200);

        var habitoComDescricao = Habito.Criar("Leitura", descricaoCom200, CorValida, _frequenciaDiaria, _hoje, _agoraUtc);
        var habitoSemDescricao = Habito.Criar("Leitura 2", null, CorValida, _frequenciaDiaria, _hoje, _agoraUtc);

        Assert.Equal(descricaoCom200, habitoComDescricao.Descricao);
        Assert.Null(habitoSemDescricao.Descricao);
    }

    [Theory]
    [InlineData("#000000")]
    [InlineData("azul")]
    [InlineData("")]
    [InlineData("#123456")]
    public void Criar_CorForaDaPaleta_LancaExcecaoDeValidacao(string corInvalida)
    {
        // 001 CA 1.5: exigir uma cor escolhida de uma paleta fixa de 8 cores
        var excecao = Assert.Throws<ExcecaoDeValidacao>(() =>
            Habito.Criar("Treino", null, corInvalida, _frequenciaDiaria, _hoje, _agoraUtc));

        Assert.Equal("cor", excecao.Campo);
    }

    [Fact]
    public void PaletaDeCores_ContemAs8CoresEspecificadas()
    {
        var coresEsperadas = new[]
        {
            "#E53935", "#FB8C00", "#FDD835", "#43A047",
            "#00ACC1", "#1E88E5", "#8E24AA", "#6D4C41"
        };

        Assert.Equal(8, PaletaDeCores.Todas.Count);
        foreach (var cor in coresEsperadas)
        {
            Assert.True(PaletaDeCores.Contem(cor));
        }
    }

    [Fact]
    public void AtualizarDados_DadosValidos_AtualizaNomeDescricaoECor()
    {
        // 001 CA 3.1: atualiza o hábito sem alterar suas marcações ou agendas
        var habito = Habito.Criar("Leitura", "Antes de dormir", CorValida, _frequenciaDiaria, _hoje, _agoraUtc);
        var novaCor = "#1E88E5";

        habito.AtualizarDados("  Leitura Noturna  ", "Capítulo diário", novaCor);

        Assert.Equal("Leitura Noturna", habito.Nome);
        Assert.Equal("Capítulo diário", habito.Descricao);
        Assert.Equal(novaCor, habito.Cor);
    }

    [Fact]
    public void AtualizarDados_DadosInvalidos_LancaExcecaoDeValidacao()
    {
        // 001 CA 3.2: mesmas validações do cadastro
        var habito = Habito.Criar("Leitura", null, CorValida, _frequenciaDiaria, _hoje, _agoraUtc);

        Assert.Throws<ExcecaoDeValidacao>(() => habito.AtualizarDados("", null, CorValida));
        Assert.Throws<ExcecaoDeValidacao>(() => habito.AtualizarDados("Novo", new string('X', 201), CorValida));
        Assert.Throws<ExcecaoDeValidacao>(() => habito.AtualizarDados("Novo", null, "#000000"));
    }
}
