namespace Habitos.Dominio;

public class ExcecaoDeRegraDeNegocio : Exception
{
    public ExcecaoDeRegraDeNegocio(string mensagem) : base(mensagem)
    {
    }
}
