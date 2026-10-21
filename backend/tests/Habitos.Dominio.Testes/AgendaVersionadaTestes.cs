namespace Habitos.Dominio.Testes;

public class AgendaVersionadaTestes
{
    private readonly DateOnly _diaCriacao = new(2026, 10, 1);
    private readonly DateTime _agoraUtc = new(2026, 10, 1, 12, 0, 0, DateTimeKind.Utc);
    private const string CorValida = "#43A047";

    [Fact]
    public void FrequenciaEm_AposAlteracao_MantemFrequenciaPassadaEAplicaNovaDeHojeEmDiante()
    {
        // 001 CA 3.3: aplicar a nova frequência a partir de hoje, inclusive, e manter a frequência anterior para os dias passados
        var habito = Habito.Criar("Correr", null, CorValida, Frequencia.Diaria(), _diaCriacao, _agoraUtc);
        var diaMudanca = new DateOnly(2026, 10, 10);
        var novaFrequencia = Frequencia.VezesNaSemana(3);

        habito.AlterarFrequencia(novaFrequencia, diaMudanca);

        // Dias passados mantêm Diária
        Assert.Equal(TipoFrequencia.Diaria, habito.FrequenciaEm(new DateOnly(2026, 10, 1)).Tipo);
        Assert.Equal(TipoFrequencia.Diaria, habito.FrequenciaEm(new DateOnly(2026, 10, 9)).Tipo);

        // A partir do dia da mudança, vale 3x por semana
        Assert.Equal(novaFrequencia, habito.FrequenciaEm(diaMudanca));
        Assert.Equal(novaFrequencia, habito.FrequenciaEm(new DateOnly(2026, 10, 15)));
    }

    [Fact]
    public void AlterarFrequencia_MesmaFrequenciaVigente_NaoFazNada()
    {
        var frequencia = Frequencia.Diaria();
        var habito = Habito.Criar("Leitura", null, CorValida, frequencia, _diaCriacao, _agoraUtc);
        var diaMudanca = new DateOnly(2026, 10, 10);

        habito.AlterarFrequencia(Frequencia.Diaria(), diaMudanca);

        Assert.Single(habito.Agendas);
        Assert.Equal(_diaCriacao, habito.Agendas.First().VigenteDesde);
    }

    [Fact]
    public void AlterarFrequencia_DuasAlteracoesNoMesmoDia_SubstituiAgendaDeHoje()
    {
        // 001 CA 3.4: SE a frequência for alterada mais de uma vez no mesmo dia, ENTÃO o sistema DEVE considerar apenas a última alteração.
        var habito = Habito.Criar("Academia", null, CorValida, Frequencia.Diaria(), _diaCriacao, _agoraUtc);
        var diaMudanca = new DateOnly(2026, 10, 10);
        var freqIntermediaria = Frequencia.NosDias(DiasDaSemana.Segunda | DiasDaSemana.Quarta);
        var freqFinal = Frequencia.VezesNaSemana(4);

        habito.AlterarFrequencia(freqIntermediaria, diaMudanca);
        habito.AlterarFrequencia(freqFinal, diaMudanca);

        Assert.Equal(2, habito.Agendas.Count);
        Assert.Equal(freqFinal, habito.FrequenciaEm(diaMudanca));
        Assert.Equal(TipoFrequencia.Diaria, habito.FrequenciaEm(new DateOnly(2026, 10, 9)).Tipo);
    }

    [Fact]
    public void AlterarFrequencia_VoltarAFrequenciaAnteriorNoMesmoDia_RemoveAgendaDeHoje()
    {
        // Design: Se ela ficar igual à da agenda anterior, remove a agenda de hoje.
        var habito = Habito.Criar("Academia", null, CorValida, Frequencia.Diaria(), _diaCriacao, _agoraUtc);
        var diaMudanca = new DateOnly(2026, 10, 10);
        var freqTemporaria = Frequencia.NosDias(DiasDaSemana.Segunda | DiasDaSemana.Quarta);

        habito.AlterarFrequencia(freqTemporaria, diaMudanca);
        Assert.Equal(2, habito.Agendas.Count);

        // Volta para Diária no mesmo dia
        habito.AlterarFrequencia(Frequencia.Diaria(), diaMudanca);

        Assert.Single(habito.Agendas);
        Assert.Equal(_diaCriacao, habito.Agendas.First().VigenteDesde);
        Assert.Equal(TipoFrequencia.Diaria, habito.FrequenciaEm(diaMudanca).Tipo);
    }

    [Fact]
    public void AlterarFrequencia_NoDiaDaCriacao_AtualizaPrimeiraAgendaSemCriarDuplicada()
    {
        var habito = Habito.Criar("Alongamento", null, CorValida, Frequencia.Diaria(), _diaCriacao, _agoraUtc);
        var novaFreq = Frequencia.NosDias(DiasDaSemana.Terca | DiasDaSemana.Quinta);

        habito.AlterarFrequencia(novaFreq, _diaCriacao);

        Assert.Single(habito.Agendas);
        var agenda = habito.Agendas.First();
        Assert.Equal(_diaCriacao, agenda.VigenteDesde);
        Assert.Equal(novaFreq, agenda.Frequencia);
    }
}
