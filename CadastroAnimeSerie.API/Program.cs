using CadastroAnimeSerie.API.Endpoints;
using CadastroAnimeSerie.Dados.Banco;
using CadastroAnimeSerie.Modelo;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CadastroAnimeSerieContext>((options) => {
    options
            .UseSqlServer(builder.Configuration["ConnectionStrings:CadastroAnimeSerieDB"])
            .UseLazyLoadingProxies();
});

builder.Services.AddSwaggerGen();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<DAL<Anime>>();
builder.Services.AddTransient<DAL<Serie>>();

var app = builder.Build();

//app.UseStaticFiles();
app.AddEndPointsAnime();
app.AddEndPointsSerie();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();

public partial class Program;