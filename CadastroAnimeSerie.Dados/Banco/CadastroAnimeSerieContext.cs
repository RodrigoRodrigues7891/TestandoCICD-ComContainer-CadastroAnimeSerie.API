using CadastroAnimeSerie.Modelo;
using Microsoft.EntityFrameworkCore;

namespace CadastroAnimeSerie.Dados.Banco;

public class CadastroAnimeSerieContext: DbContext
{
    public DbSet<Anime> Animes { get; set; }
    public DbSet<Serie> Series { get; set; }

    private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CadastroAnimeSerieV0;Integrated Security=True;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

    public CadastroAnimeSerieContext()
    {

    }
    public CadastroAnimeSerieContext(DbContextOptions options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }
        optionsBuilder
            .UseSqlServer(connectionString)
            .UseLazyLoadingProxies();
    }
}
