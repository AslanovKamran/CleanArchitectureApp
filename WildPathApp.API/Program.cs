using WildPathApp.Infrastructure.Database;
using WildPathApp.API.Extensions;
using Microsoft.OpenApi.Models;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen((options) =>
{
    #region Documentation Section

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Wild Path",
        Version = "v1",
        Description = "API documentation"
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    #endregion
});

// Register DatabaseConnection as a singleton
builder.Services.AddSingleton<DatabaseConnection>();

// Extension methods to register repositories and services from the API layer
builder.Services.AddRepositories();
builder.Services.AddServices();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.DisplayRequestDuration(); });

}

app.UseCors(options =>
{
    options.AllowAnyMethod();
    options.AllowAnyHeader();
    options.AllowAnyOrigin();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
