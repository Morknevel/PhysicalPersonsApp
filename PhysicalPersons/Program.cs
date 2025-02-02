
using Bal.ServiceContracts;
using Bal.Services;
using Dal.Contracts;
using Dal.Data;
using Dal.Repositories;
using Microsoft.EntityFrameworkCore;
using PhysicalPersons.Configurations;
using PhysicalPersons.CustomMiddlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProjectServices(builder.Configuration);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ErrorLoggingMiddleware>();

app.MapDefaultControllerRoute();

app.Run();
