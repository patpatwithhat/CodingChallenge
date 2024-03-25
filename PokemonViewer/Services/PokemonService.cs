using Newtonsoft.Json;
using PokemonViewer.Models;
using System.Net.Http;

namespace PokemonViewer.Services
{
    public class PokemonService
    {
        public async Task<PokemonModel?> GetPokemonDetails(string pokemonName)
        {
            using (var client = new HttpClient())
            {
                string url = $"https://pokeapi.co/api/v2/pokemon/{pokemonName.ToLower()}";
                var response = await client.GetStringAsync(url);
        
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
        }
    }
}
