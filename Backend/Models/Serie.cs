namespace Backend.Models
{
    public class Serie
    {
        public int IdSerie { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Sinopse { get; set; } = string.Empty;
        public string Foto { get; set; } = string.Empty;
        public string Estudio { get; set; } = string.Empty;
        public int Ano { get; set; }
        public int QtdeEpisodios { get => Episodios.Count; }
        public bool Lancamento { get; set; }
        public List<string> Categorias { get; set; } = new();
        public List<string> Episodios { get; set; } = new();
    }
}
