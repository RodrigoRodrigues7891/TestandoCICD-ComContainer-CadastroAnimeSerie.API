namespace CadastroAnimeSerie.Modelo;

public class Serie
{
    public Serie(string nome, string sinopse, int? quantidadeDeEpisodios, int? anoDoLancamento, string? diretor)
    {
        Nome = nome;
        Sinopse = sinopse;
        QuantidadeDeEpisodios = quantidadeDeEpisodios;
        AnoDoLancamento = anoDoLancamento;
        Diretor = diretor;
    }
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Sinopse { get; set; }
    public int? QuantidadeDeEpisodios { get; set; }
    public int? AnoDoLancamento { get; set; }
    public string? Diretor { get; set; }
}
