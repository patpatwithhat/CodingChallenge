using Newtonsoft.Json;
using PokemonViewer.Models;
using System.Net.Http;

namespace PokemonViewer.Services
{
    public class PokemonService
    {
        private readonly HttpClient _httpClient;
        public PokemonService() { _httpClient = new HttpClient(); }
        public PokemonService(HttpClient httpClient) { _httpClient = httpClient; }

        public async Task<PokemonModel?> GetPokemonDetails(string pokemonName)
        {
            try
            {
                string url = $"https://pokeapi.co/api/v2/pokemon/{pokemonName.ToLower()}";
                var response = await _httpClient.GetStringAsync(url);

                dynamic? pokemonJson = JsonConvert.DeserializeObject(response);
                if (pokemonJson == null)
                    return null;

                var pokemon = new PokemonModel
                {
                    Name = pokemonJson.name,
                    Abilities = new List<string>(),
                    Types = new List<string>(),
                    ImageUrl = pokemonJson.sprites.front_default,
                    ShinyImageUrl = pokemonJson.sprites.front_shiny
                };

                foreach (var ability in pokemonJson.abilities)
                {
                    pokemon.Abilities.Add(ability.ability.name.ToString());
                }

                foreach (var type in pokemonJson.types)
                {
                    pokemon.Types.Add(type.type.name.ToString());
                }

                return pokemon;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
    }
}
