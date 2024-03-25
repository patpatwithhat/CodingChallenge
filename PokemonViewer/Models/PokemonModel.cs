namespace PokemonViewer.Models
{
    public class PokemonModel
    {
        public string? Name { get; set; }
        public List<string>? Abilities { get; set; }
        public List<string>? Types { get; set; }
        public string? ImageUrl { get; set; }
        public string? ShinyImageUrl { get; set; }
    }
}
