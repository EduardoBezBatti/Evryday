namespace Habitos.Dominio;

[Flags]
public enum DiasDaSemana
{
    Nenhum = 0,
    Segunda = 1,
    Terca = 2,
    Quarta = 4,
    Quinta = 8,
    Sexta = 16,
    Sabado = 32,
    Domingo = 64
}
