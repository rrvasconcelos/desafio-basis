using System.Reflection;
using BookStore.Api;
using BookStore.Api.Extensions;
using BookStore.Application;
using BookStore.Infrastructure;

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

app.UseCors("AllowAllOrigins");

app.MapEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
    //add migration
}

app.UseExceptionHandler();

await app.RunAsync();