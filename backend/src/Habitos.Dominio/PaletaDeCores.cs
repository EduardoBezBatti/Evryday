namespace Habitos.Dominio;

public static class PaletaDeCores
{
    public static readonly IReadOnlyList<string> Todas = new[]
    {
        "#E53935", // Vermelho
        "#FB8C00", // Laranja
        "#FDD835", // Amarelo
        "#43A047", // Verde
        "#00ACC1", // Ciano
        "#1E88E5", // Azul
        "#8E24AA", // Roxo
        "#6D4C41"  // Marrom
    };

    private static readonly Dictionary<string, string> MapeamentoNomes = new(StringComparer.OrdinalIgnoreCase)
    {
        { "vermelho", "#E53935" },
        { "laranja", "#FB8C00" },
        { "amarelo", "#FDD835" },
        { "verde", "#43A047" },
        { "ciano", "#00ACC1" },
        { "azul", "#1E88E5" },
        { "roxo", "#8E24AA" },
        { "marrom", "#6D4C41" }
    };

    public static bool Contem(string cor) =>
        Todas.Any(c => string.Equals(c, cor, StringComparison.OrdinalIgnoreCase)) ||
        MapeamentoNomes.ContainsKey(cor);

    public static string Traduzir(string cor)
    {
        if (MapeamentoNomes.TryGetValue(cor, out var hex))
        {
            return hex;
        }

        return cor.ToUpperInvariant();
    }
}
