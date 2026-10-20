namespace Habitos.Dominio.Testes;

public class SemanaTestes
{
    [Theory]
    [InlineData(2026, 10, 19)] // Segunda-feira
    [InlineData(2026, 10, 20)] // Terça-feira
    [InlineData(2026, 10, 21)] // Quarta-feira
    [InlineData(2026, 10, 22)] // Quinta-feira
    [InlineData(2026, 10, 23)] // Sexta-feira
    [InlineData(2026, 10, 24)] // Sábado
    [InlineData(2026, 10, 25)] // Domingo
    public void InicioDe_QualquerDiaDaSemana_RetornaSegundaFeiraCorrespondente(int ano, int mes, int dia)
    {
        // 001 CA 7.2: O sistema DEVE considerar que a semana vai de segunda a domingo.
        var data = new DateOnly(ano, mes, dia);
        var segundaEsperada = new DateOnly(2026, 10, 19);

        var inicio = Semana.InicioDe(data);

        Assert.Equal(segundaEsperada, inicio);
        Assert.Equal(DayOfWeek.Monday, inicio.DayOfWeek);
    }

    [Fact]
    public void InicioDe_ViradaDeAno_RetornaSegundaFeiraNoAnoAnterior()
    {
        // 2026-01-01 é uma Quinta-feira. A segunda-feira daquela semana foi 2025-12-29.
        var primeiroDeJaneiro = new DateOnly(2026, 1, 1);
        var segundaEsperada = new DateOnly(2025, 12, 29);

        var inicio = Semana.InicioDe(primeiroDeJaneiro);

        Assert.Equal(segundaEsperada, inicio);
    }
}
