using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using PokemonViewer.Services;
using System.Net;

namespace PokemonViewer.Tests
{
    public class PokemonServiceTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly PokemonService _pokemonService;

        public PokemonServiceTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _pokemonService = new PokemonService(_httpClient);
        }

        [Fact]
        public async Task GetPokemonDetails_ReturnsPokemonModel_WhenPokemonExists()
        {
            // Arrange
            var pokemonName = "pikachu";
            var pokemonJson = new
            {
                name = "pikachu",
                abilities = new[]
                {
                    new { ability = new { name = "static" } },
                    new { ability = new { name = "lightning-rod" } }
                },
                types = new[]
                {
                    new { type = new { name = "electric" } }
                },
                sprites = new
                {
                    front_default = "http://example.com/front_default.png",
                    front_shiny = "http://example.com/front_shiny.png"
                }
            };

            var responseJson = JsonConvert.SerializeObject(pokemonJson);

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseJson)
                });

            // Act
            var result = await _pokemonService.GetPokemonDetails(pokemonName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("pikachu", result.Name);
            Assert.Equal(2, result.Abilities.Count);
            Assert.Contains("static", result.Abilities);
            Assert.Contains("lightning-rod", result.Abilities);
            Assert.Equal(1, result.Types.Count);
            Assert.Contains("electric", result.Types);
            Assert.Equal("http://example.com/front_default.png", result.ImageUrl);
            Assert.Equal("http://example.com/front_shiny.png", result.ShinyImageUrl);
        }

        [Fact]
        public async Task GetPokemonDetails_ReturnsNull_WhenPokemonDoesNotExist()
        {
            // Arrange
            var pokemonName = "nonexistentpokemon";

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

            // Act
            var result = await _pokemonService.GetPokemonDetails(pokemonName);

            // Assert
            Assert.Null(result);
        }
    }
}
