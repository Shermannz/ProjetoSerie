namespace Backend.Models.dto
{
    public class ListaSerieDTO
    {
        public int PaginaAtual { get; set; }
        public int PaginaTotais { get; set; }
        public IEnumerable<Serie> Series { get; set; } = new List<Serie>();
    }
}
