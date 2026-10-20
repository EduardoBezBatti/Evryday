namespace Habitos.Dominio.Testes;

public class FrequenciaTestes
{
    [Fact]
    public void Diaria_CriaFrequenciaDiariaValida()
    {
        var frequencia = Frequencia.Diaria();

        Assert.Equal(TipoFrequencia.Diaria, frequencia.Tipo);
        Assert.Equal(DiasDaSemana.Nenhum, frequencia.Dias);
        Assert.Null(frequencia.VezesPorSemana);
    }

    [Fact]
    public void NosDias_SemNenhumDia_LancaExcecaoDeValidacao()
    {
        // 001 CA 1.7: QUANDO o tipo de frequência for "dias da semana", o sistema DEVE exigir ao menos um dia selecionado.
        var excecao = Assert.Throws<ExcecaoDeValidacao>(() => Frequencia.NosDias(DiasDaSemana.Nenhum));

        Assert.Equal("dias", excecao.Campo);
    }

    [Fact]
    public void NosDias_ComDiasValidos_CriaFrequenciaComDias()
    {
        var dias = DiasDaSemana.Segunda | DiasDaSemana.Quarta | DiasDaSemana.Sexta;
        var frequencia = Frequencia.NosDias(dias);

        Assert.Equal(TipoFrequencia.DiasDaSemana, frequencia.Tipo);
        Assert.Equal(dias, frequencia.Dias);
        Assert.Null(frequencia.VezesPorSemana);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(7)]
    [InlineData(10)]
    public void VezesNaSemana_MetaForaDoIntervaloDe1a6_LancaExcecaoDeValidacao(int metaInvalida)
    {
        // 001 CA 1.8: QUANDO o tipo de frequência for "vezes por semana", o sistema DEVE exigir uma meta inteira de 1 a 6.
        var excecao = Assert.Throws<ExcecaoDeValidacao>(() => Frequencia.VezesNaSemana(metaInvalida));

        Assert.Equal("vezesPorSemana", excecao.Campo);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(6)]
    public void VezesNaSemana_MetaValida_CriaFrequenciaComMeta(int metaValida)
    {
        var frequencia = Frequencia.VezesNaSemana(metaValida);

        Assert.Equal(TipoFrequencia.VezesPorSemana, frequencia.Tipo);
        Assert.Equal(DiasDaSemana.Nenhum, frequencia.Dias);
        Assert.Equal(metaValida, frequencia.VezesPorSemana);
    }

    [Fact]
    public void PreveDia_FrequenciaDiaria_RetornaVerdadeiroParaQualquerData()
    {
        // 001 CA 5.2: diários preveem todos os dias
        var frequencia = Frequencia.Diaria();
        var terca = new DateOnly(2026, 10, 20); // Terça-feira
        var domingo = new DateOnly(2026, 10, 25); // Domingo

        Assert.True(frequencia.PreveDia(terca));
        Assert.True(frequencia.PreveDia(domingo));
    }

    [Fact]
    public void PreveDia_VezesPorSemana_RetornaVerdadeiroParaQualquerData()
    {
        // 001 CA 5.2: vezes por semana preveem todos os dias
        var frequencia = Frequencia.VezesNaSemana(3);
        var terca = new DateOnly(2026, 10, 20);
        var sabado = new DateOnly(2026, 10, 24);

        Assert.True(frequencia.PreveDia(terca));
        Assert.True(frequencia.PreveDia(sabado));
    }

    [Fact]
    public void PreveDia_DiasDaSemana_RetornaVerdadeiroApenasNosDiasEscolhidos()
    {
        // 001 CA 5.2: dias da semana só os dias escolhidos
        // 2026-10-19 é Segunda
        // 2026-10-20 é Terça
        // 2026-10-21 é Quarta
        // 2026-10-22 é Quinta
        // 2026-10-23 é Sexta
        // 2026-10-24 é Sábado
        // 2026-10-25 é Domingo
        var frequencia = Frequencia.NosDias(DiasDaSemana.Segunda | DiasDaSemana.Quarta | DiasDaSemana.Sexta);

        Assert.True(frequencia.PreveDia(new DateOnly(2026, 10, 19))); // Segunda
        Assert.False(frequencia.PreveDia(new DateOnly(2026, 10, 20))); // Terça
        Assert.True(frequencia.PreveDia(new DateOnly(2026, 10, 21))); // Quarta
        Assert.False(frequencia.PreveDia(new DateOnly(2026, 10, 22))); // Quinta
        Assert.True(frequencia.PreveDia(new DateOnly(2026, 10, 23))); // Sexta
        Assert.False(frequencia.PreveDia(new DateOnly(2026, 10, 24))); // Sábado
        Assert.False(frequencia.PreveDia(new DateOnly(2026, 10, 25))); // Domingo
    }
}
