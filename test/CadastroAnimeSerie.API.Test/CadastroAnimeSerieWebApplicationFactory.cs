using CadastroAnimeSerie.Dados.Banco;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace CadastroAnimeSerie.API.Test;

public class CadastroAnimeSerieWebApplicationFactory: WebApplicationFactory<Program>, IAsyncLifetime
{
    public CadastroAnimeSerieContext Context { get; private set; }
    private IServiceScope scope;
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(s => 
            s.ServiceType == typeof(DbContextOptions<CadastroAnimeSerieContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<CadastroAnimeSerieContext>(options =>
            {
                options
                    .UseSqlServer(_msSqlContainer.GetConnectionString());
            });
        });
    }

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync(); //levanda o container no docker
        this.scope = Services.CreateScope(); //vai pro "Program.cs" pegas as configurações dentre elas o banco local
        Context = scope.ServiceProvider.GetRequiredService<CadastroAnimeSerieContext>(); //Joga o contexto do
                                                                                         //escopo atual que é o container
        Context.Database.Migrate(); //Roda as migrations pro contexto atual, container
    }

    public async Task DisposeAsync()
    {
        await _msSqlContainer.StopAsync();
    }
}
