namespace Habitos.Dominio;

public static class Semana
{
    public static DateOnly InicioDe(DateOnly data)
    {
        var diasDesdeSegunda = (7 + (int)data.DayOfWeek - (int)DayOfWeek.Monday) % 7;
        return data.AddDays(-diasDesdeSegunda);
    }
}
