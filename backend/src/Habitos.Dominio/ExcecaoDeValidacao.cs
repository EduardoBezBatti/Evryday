namespace Habitos.Dominio;

public class ExcecaoDeValidacao : Exception
{
    public string Campo { get; }

    public ExcecaoDeValidacao(string campo, string mensagem) : base(mensagem)
    {
        Campo = campo;
    }
}
