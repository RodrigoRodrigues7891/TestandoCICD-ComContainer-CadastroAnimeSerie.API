using Bogus;
using CadastroAnimeSerie.Modelo;

namespace CadastroAnimeSerie.API.Test.DataBuilders;

public class AnimeDataBuilder : Faker<Anime>
{
    public string Nome { get; set; }
    public string Sinopse { get; set; }
    public int? QuantidadeDeEpisodios { get; set; }
    public int? AnoDoLancamento { get; set; }
    public string? Diretor { get; set; }
    public AnimeDataBuilder()
    {
        CustomInstantiator(f =>
        {
            string nome = Nome ?? string.Join(" ", f.Lorem.Words(2));
            string sinopse = Sinopse ?? f.Lorem.Sentences();
            int quantidadeEpisodios = QuantidadeDeEpisodios ?? f.Random.Int(5, 100);
            int anoDoLancamento = AnoDoLancamento ?? f.Random.Int(1990, 2025);
            string diretor = Diretor ?? string.Join(" ", f.Lorem.Words(2));

            return new Anime(nome, sinopse, quantidadeEpisodios, anoDoLancamento, diretor);
        });
    }
}
