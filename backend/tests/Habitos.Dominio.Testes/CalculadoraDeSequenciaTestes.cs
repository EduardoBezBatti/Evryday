using Habitos.Dominio;
using Xunit;

namespace Habitos.Dominio.Testes;

public class CalculadoraDeSequenciaTestes
{
    [Fact]
    public void CalcularAtual_HabitoDiario_MantemSequenciaQuandoMarcado()
    {
        // Arrange
        var inicio = new DateOnly(2026, 10, 1);
        var hoje = new DateOnly(2026, 10, 3);
        var habito = Habito.Criar("Teste", null, "#43A047", Frequencia.Diaria(), inicio, DateTime.UtcNow);

        habito.AdicionarMarcacaoParaTeste(new DateOnly(2026, 10, 1), DateTime.UtcNow);
        habito.AdicionarMarcacaoParaTeste(new DateOnly(2026, 10, 2), DateTime.UtcNow);
        habito.AdicionarMarcacaoParaTeste(new DateOnly(2026, 10, 3), DateTime.UtcNow);

        // Act
        var resultado = CalculadoraDeSequencia.Calcular(habito, hoje);

        // Assert
        Assert.Equal(3, resultado.Atual);
    }

    [Fact]
    public void CalcularAtual_HabitoDiario_QuebraSequenciaQuandoNaoMarcado()
    {
        // Arrange
        var inicio = new DateOnly(2026, 10, 1);
        var hoje = new DateOnly(2026, 10, 3);
        var habito = Habito.Criar("Teste", null, "#43A047", Frequencia.Diaria(), inicio, DateTime.UtcNow);

        habito.AdicionarMarcacaoParaTeste(new DateOnly(2026, 10, 1), DateTime.UtcNow);
        // 10/02 não marcado
        habito.AdicionarMarcacaoParaTeste(new DateOnly(2026, 10, 3), DateTime.UtcNow);

        // Act
        var resultado = CalculadoraDeSequencia.Calcular(habito, hoje);

        // Assert
        Assert.Equal(1, resultado.Atual);
    }

    [Fact]
    public void CalcularAtual_HabitoVezesPorSemana_MantemSequenciaSeMetaBatida()
    {
        // Arrange
        // Meta: 2 vezes por semana
        var freq = Frequencia.VezesNaSemana(2);
        var inicio = new DateOnly(2026, 10, 1); 
        var hoje = new DateOnly(2026, 10, 12); // Segunda
        var habito = Habito.Criar("Teste", null, "#43A047", freq, inicio, DateTime.UtcNow);

        // Semana Anterior (05/10 a 11/10): Marcado Terça(6) e Quinta(8) -> Meta 2 batida.
        habito.AdicionarMarcacaoParaTeste(new DateOnly(2026, 10, 6), DateTime.UtcNow);
        habito.AdicionarMarcacaoParaTeste(new DateOnly(2026, 10, 8), DateTime.UtcNow);

        // Semana Atual (12/10): Marcado Segunda(12) -> Em progresso.
        habito.AdicionarMarcacaoParaTeste(new DateOnly(2026, 10, 12), DateTime.UtcNow);

        // Act
        var resultado = CalculadoraDeSequencia.Calcular(habito, hoje);

        // Assert
        // Deve ter 1 semana completa de sequência.
        Assert.Equal(1, resultado.Atual);
    }

    [Fact]
    public void CalcularAtual_HabitoVezesPorSemana_QuebraSequenciaSeMetaNaoBatida()
    {
        // Arrange
        // Meta: 2 vezes por semana
        var freq = Frequencia.VezesNaSemana(2);
        var inicio = new DateOnly(2026, 10, 1);
        var hoje = new DateOnly(2026, 10, 12);
        var habito = Habito.Criar("Teste", null, "#43A047", freq, inicio, DateTime.UtcNow);

        // Semana 1: Marcado apenas Quarta(1) -> Meta 2 NÃO batida.
        habito.AdicionarMarcacaoParaTeste(new DateOnly(2026, 10, 1), DateTime.UtcNow);

        // Semana 2: Segunda(1).
        habito.AdicionarMarcacaoParaTeste(new DateOnly(2026, 10, 12), DateTime.UtcNow);

        // Act
        var resultado = CalculadoraDeSequencia.Calcular(habito, hoje);

        // Assert
        Assert.Equal(0, resultado.Atual);
    }
}
