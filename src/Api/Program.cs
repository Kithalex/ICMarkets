using Application.Features.BlockchainHistory;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

builder.Services.AddHealthChecks();

builder.Services.AddMediatR(typeof(GetBlockchainHistoryQuery).Assembly);
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseCors();

app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health", new HealthCheckOptions());

app.Run();

public partial class Program { }
