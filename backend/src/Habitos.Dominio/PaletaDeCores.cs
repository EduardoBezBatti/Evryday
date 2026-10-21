namespace Habitos.Dominio;

public static class PaletaDeCores
{
    public static readonly IReadOnlyList<string> Todas = new[]
    {
        "#E53935",
        "#FB8C00",
        "#FDD835",
        "#43A047",
        "#00ACC1",
        "#1E88E5",
        "#8E24AA",
        "#6D4C41"
    };

    public static bool Contem(string cor) =>
        Todas.Any(c => string.Equals(c, cor, StringComparison.OrdinalIgnoreCase));
}
