using AutoMapper;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using RbiApi;
using RbiData;
using RbiData.DAOs;
using RbiData.Entities;
using RbiData.Services;
using RbiShared.DTOs;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var allowClientAppOrigin = "_allowApiOrigin";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowClientAppOrigin,
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:7098")
                                .AllowAnyMethod()
                                .WithHeaders(HeaderNames.ContentType);
                      });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IManagedTransactionFactory, ManagedTransactionFactory>();
builder.Services.AddSingleton<IDaoFactory<RecipeDao>, RecipeDaoFactory>();
builder.Services.AddSingleton<IRecipeService, RecipeService>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.EnableTryItOutByDefault();
    });
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseCors(allowClientAppOrigin);

app.UseAuthorization();

app.MapControllers();

app.Run();
