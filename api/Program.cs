using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Middleware;
using Domain.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<InvestmentRepository>(options => options.UseInMemoryDatabase("InvestmentDB"));
builder.Services.AddScoped<IInvestmentRepository, InvestmentRepository>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
app.UseCors();

app.MapGet("/", () => "Hello World!");

var InvestmentApiGroup = app.MapGroup("/api/investments");

InvestmentApiGroup.MapGet("/", async ([FromServices] IInvestmentRepository repo) =>
{
    var investments = await repo.GetAllInvestmentsAsync();
    return Results.Ok(investments);
});

InvestmentApiGroup.MapGet("/{id}", async (int id, [FromServices] IInvestmentRepository repo) =>
{
    if (id <= 0)
    {
        return Results.BadRequest(new ErrorResponse("Invalid investment ID", 400, "Investment ID must be greater than 0"));
    }

    var investment = await repo.GetInvestmentByIdAsync(id);
    return Results.Ok(investment);
});

InvestmentApiGroup.MapGet("/{id}/tvpi", async (int id, [FromServices] IInvestmentRepository repo) =>
{
    if (id <= 0)
    {
        return Results.BadRequest(new ErrorResponse("Invalid investment ID", 400, "Investment ID must be greater than 0"));
    }

    var investment = await repo.GetInvestmentByIdAsync(id);
    var tvpi = investment.CalculateTVPI();
    
    return Results.Ok(new { InvestmentId = id, TVPI = tvpi });
});

InvestmentApiGroup.MapPost("/", async (Investment investment, [FromServices] IInvestmentRepository repo) =>
{
    if (investment == null)
    {
        return Results.BadRequest(new ErrorResponse("Invalid investment data", 400, "Investment object cannot be null"));
    }

    if (string.IsNullOrWhiteSpace(investment.Name))
    {
        return Results.BadRequest(new ErrorResponse("Invalid investment data", 400, "Investment name is required"));
    }

    if (investment.CommittedCapital < 0)
    {
        return Results.BadRequest(new ErrorResponse("Invalid investment data", 400, "Committed capital cannot be negative"));
    }

    var createdInvestment = await repo.AddInvestmentAsync(investment);
    return Results.Created($"/api/investments/{createdInvestment.Id}", createdInvestment);
});

InvestmentApiGroup.MapDelete("/{id}", async (int id, [FromServices] IInvestmentRepository repo) =>
{
    if (id <= 0)
    {
        return Results.BadRequest(new ErrorResponse("Invalid investment ID", 400, "Investment ID must be greater than 0"));
    }

    await repo.DeleteInvestmentAsync(id);
    return Results.NoContent();
});

app.Run();
