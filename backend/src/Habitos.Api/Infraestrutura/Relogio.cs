using Habitos.Dominio.Interfaces;

namespace Habitos.Api.Infraestrutura;

public class Relogio : IRelogio
{
    private readonly TimeProvider _timeProvider;

    public Relogio(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    public DateOnly Hoje()
    {
        // TimeProvider.GetLocalNow() retorna DateTimeOffset.
        // Convertemos para DateTime para então usar DateOnly.
        return DateOnly.FromDateTime(_timeProvider.GetLocalNow().DateTime);
    }
}
