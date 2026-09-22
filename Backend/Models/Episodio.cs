namespace Backend.Models
{
    public class Episodio
    {
        public int IdEpisodio { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Video { get; set; } = string.Empty;
        public int IdSerie { get; set; }
        public string Serie { get; set; } = string.Empty;

    }
}
