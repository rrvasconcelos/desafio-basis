using System.Reflection;
using BookStore.Api;
using BookStore.Api.Extensions;
using BookStore.Application;
using BookStore.Infrastructure;
using BookStore.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder
            .AllowAnyOrigin()  
            .AllowAnyMethod() 
            .AllowAnyHeader());
});

builder.Services
    .AddApplication()
    .AddPresentation()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

var app = builder.Build();

app.MigrateDatabase();

app.UseCors("AllowAllOrigins");

app.MapEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();

await app.RunAsync();