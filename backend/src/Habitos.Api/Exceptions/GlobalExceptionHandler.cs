using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Habitos.Api.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is Habitos.Dominio.ExcecaoDeValidacao exVal)
        {
            var problem = new ValidationProblemDetails
            {
                Status = (int)HttpStatusCode.BadRequest,
                Title = "Erro de validação",
                Detail = "Um ou mais campos estão inválidos."
            };
            problem.Errors.Add(exVal.Campo, new[] { exVal.Message });
            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
            return true;
        }

        if (exception is Habitos.Dominio.ExcecaoDeRegraDeNegocio exRegra)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
            await httpContext.Response.WriteAsJsonAsync(new { 
                title = "Regra de negócio violada", 
                detail = exRegra.Message, 
                status = (int)HttpStatusCode.UnprocessableEntity 
            }, cancellationToken);
            return true;
        }

        return false; // Deixa o middleware padrão lidar com erros inesperados (500)
    }
}
