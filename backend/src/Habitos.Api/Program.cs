using Habitos.Api.Infraestrutura;
using Habitos.Api.Exceptions;
using Habitos.Api.Contratos;
using Habitos.Dominio;
using Habitos.Dominio.Interfaces;
using Habitos.Infraestrutura;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Banco de Dados (SQLite)
builder.Services.AddDbContext<HabitosDbContext>(options =>
    options.UseSqlite("Data Source=habitos.db"));

// Configuração do Relógio (TimeProvider é nativo do .NET 8+)
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddSingleton<IRelogio, Relogio>();

// Tratamento de Erros
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Ativando Swagger globalmente para garantir acesso durante o desenvolvimento
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Evryday API v1");
    c.RoutePrefix = "swagger"; 
});

app.UseExceptionHandler();

app.MapGet("/", () => "Hello World!")
    .WithTags("Sistema");

app.MapGet("/api/saude", () => Results.Ok(new { status = "API ok" }))
    .WithTags("Sistema");

// --- T3.3 Cadastro e Consulta ---

app.MapPost("/api/habitos", async (HabitoRequest req, HabitosDbContext db, IRelogio relogio) =>
{
    var hoje = relogio.Hoje();
    var agoraUtc = DateTime.UtcNow;

    var freqDefault = Habitos.Dominio.Frequencia.Diaria();

    var habito = Habito.Criar(
        req.Nome,
        req.Descricao,
        req.Cor,
        freqDefault,
        hoje,
        agoraUtc
    );

    if (await db.Habitos.AnyAsync(h => h.NomeNormalizado == habito.NomeNormalizado))
    {
        return Results.Problem(
            detail: $"O hábito '{req.Nome}' já existe. Por favor, escolha um nome único para diferenciar seus hábitos.",
            statusCode: 400,
            title: "Nome duplicado"
        );
    }

    db.Habitos.Add(habito);
    await db.SaveChangesAsync();

    return Results.Created($"/api/habitos/{habito.Id}", new HabitoResponse(
        habito.Id,
        habito.Nome,
        habito.Descricao ?? "",
        habito.Cor,
        habito.DataInicio,
        "",
        0,
        0
    ));
})
.WithTags("Hábitos");

app.MapGet("/api/habitos", async (HabitosDbContext db, IRelogio relogio) =>
{
    var hoje = relogio.Hoje();
    var habitos = await db.Habitos.ToListAsync();
    return Results.Ok(habitos.Select(h => new HabitoResponse(
        h.Id,
        h.Nome,
        h.Descricao ?? "",
        h.Cor,
        h.DataInicio,
        "",
        CalculadoraDeSequencia.Calcular(h, hoje).Atual,
        CalculadoraDeSequencia.Calcular(h, hoje).Maior
    )));
})
.WithTags("Hábitos");

app.MapGet("/api/habitos/{id:guid}", async (Guid id, HabitosDbContext db, IRelogio relogio) =>
{
    var hoje = relogio.Hoje();
    var habito = await db.Habitos.FindAsync(id);
    if (habito == null) return Results.NotFound();

    return Results.Ok(new HabitoResponse(
        habito.Id,
        habito.Nome,
        habito.Descricao ?? "",
        habito.Cor,
        habito.DataInicio,
        "",
        CalculadoraDeSequencia.Calcular(habito, hoje).Atual,
        CalculadoraDeSequencia.Calcular(habito, hoje).Maior
    ));
})
.WithTags("Hábitos");

// --- T3.4 Edição ---

app.MapPut("/api/habitos/{id:guid}", async (Guid id, HabitoUpdateRequest req, HabitosDbContext db, IRelogio relogio) =>
{
    var habito = await db.Habitos.FindAsync(id);
    if (habito == null) return Results.NotFound();

    var hoje = relogio.Hoje();

    habito.AtualizarDados(req.Nome, req.Descricao, req.Cor);
    var nomeNormalizado = habito.NomeNormalizado;
    if (await db.Habitos.AnyAsync(h => h.NomeNormalizado == nomeNormalizado && h.Id != id))
    {
        return Results.Problem(
            detail: $"O nome '{req.Nome}' já está sendo usado por outro hábito. Por favor, use um nome diferente.",
            statusCode: 400,
            title: "Nome duplicado"
        );
    }

    Habitos.Dominio.Frequencia freq = req.Frequencia.Tipo switch
    {
        0 => Habitos.Dominio.Frequencia.Diaria(),
        1 => Habitos.Dominio.Frequencia.NosDias((Habitos.Dominio.DiasDaSemana)req.Frequencia.Dias),
        2 => Habitos.Dominio.Frequencia.VezesNaSemana(req.Frequencia.VezesPorSemana ?? 1),
        _ => throw new ArgumentException("Tipo de frequência inválido")
    };

    habito.AlterarFrequencia(freq, hoje);

    await db.SaveChangesAsync();
    return Results.NoContent();
})
.WithTags("Hábitos");

// --- T3.5 Exclusão ---

app.MapDelete("/api/habitos/{id:guid}", async (Guid id, HabitosDbContext db) =>
{
    var habito = await db.Habitos.FindAsync(id);
    if (habito == null) return Results.NotFound();

    db.Habitos.Remove(habito);
    await db.SaveChangesAsync();

    return Results.NoContent();
})
.WithTags("Hábitos");

// --- T3.6 Endpoint da tela "Hoje" ---

app.MapGet("/api/hoje", async (HabitosDbContext db, IRelogio relogio) =>
{
    var hoje = relogio.Hoje();
    var todosHabitos = await db.Habitos.ToListAsync();

    var paraHoje = new List<HabitoHojeDto>();
    var extras = new List<HabitoHojeDto>();
    int marcadosHoje = 0;
    int previstosHoje = 0;

    foreach (var habito in todosHabitos)
    {
        var freqVigente = habito.FrequenciaEm(hoje);
        bool previsto = freqVigente.PreveDia(hoje);
        bool marcado = habito.EstaMarcadoEm(hoje);

        if (previsto)
        {
            previstosHoje++;
            if (marcado) marcadosHoje++;
            paraHoje.Add(new HabitoHojeDto(habito.Id, habito.Nome, habito.Cor, marcado, false, CalculadoraDeSequencia.Calcular(habito, hoje).Atual));
        }
        else if (marcado)
        {
            extras.Add(new HabitoHojeDto(habito.Id, habito.Nome, habito.Cor, true, true, CalculadoraDeSequencia.Calcular(habito, hoje).Atual));
        }
    }

    double progresso = previstosHoje == 0 ? 0 : (double)marcadosHoje / previstosHoje;

    return Results.Ok(new HojeResponse(
        paraHoje,
        extras,
        progresso,
        progresso >= 1.0 && previstosHoje > 0
    ));
})
.WithTags("Hoje");

// --- T3.7 Marcações ---

app.MapGet("/api/habitos/{id:guid}/historico", async (Guid id, HabitosDbContext db) =>
{
    var habito = await db.Habitos.FindAsync(id);
    if (habito == null) return Results.NotFound();

    var marcacoes = await db.Marcacoes
        .Where(m => m.HabitoId == id)
        .OrderBy(m => m.Data)
        .Select(m => m.Data.ToString("yyyy-MM-dd"))
        .ToListAsync();

    return Results.Ok(new {
        dataInicio = habito.DataInicio.ToString("yyyy-MM-dd"),
        dataFim = DateOnly.FromDateTime(DateTime.UtcNow),
        marcacoes = marcacoes
    });
})
.WithTags("Marcações");

app.MapPut("/api/habitos/{id:guid}/marcacoes/{data}", async (Guid id, DateOnly data, HabitosDbContext db, IRelogio relogio) =>
{
    var hoje = relogio.Hoje();
    if (data != hoje)
    {
        return Results.Problem(
            detail: $"A marcação de hábitos é estritamente honesta: você só pode marcar o dia de hoje ({hoje:yyyy-MM-dd}). Não é permitido retroceder ou adiantar marcações.",
            statusCode: 422,
            title: "Data inválida"
        );
    }

    var habito = await db.Habitos.FindAsync(id);
    if (habito == null) return Results.NotFound();

    habito.Marcar(data, hoje, DateTime.UtcNow);
    await db.SaveChangesAsync();

    return Results.NoContent();
})
.WithTags("Marcações");

app.MapDelete("/api/habitos/{id:guid}/marcacoes/{data}", async (Guid id, DateOnly data, HabitosDbContext db, IRelogio relogio) =>
{
    var hoje = relogio.Hoje();
    if (data != hoje)
    {
        return Results.Problem(
            detail: $"A desmarcação de hábitos é estritamente honesta: você só pode desmarcar o dia de hoje ({hoje:yyyy-MM-dd}).",
            statusCode: 422,
            title: "Data inválida"
        );
    }

    var habito = await db.Habitos.FindAsync(id);
    if (habito == null) return Results.NotFound();

    habito.Desmarcar(data, hoje);
    await db.SaveChangesAsync();

    return Results.NoContent();
})
.WithTags("Marcações");

app.Run();

public partial class Program { }
