using Moq;
using PokemonAPI.Services;
using System.Linq;
using Xunit;

namespace PokemonAPI.Tests
{
    public class UnitTest1
    {

        private Mock<HttpClient> _mockHttpClient = new Mock<HttpClient>();
        private Dictionary<string, object> _cache = new Dictionary<string, object>();


        [Fact]
        public void GetAllPokemonsByValidAbility()
        {
            var abilityName = "levitate";
            var expectedPokemonNames = new List<string> { "gastly", "haunter", "koffing", "weezing", "misdreavus" };
            _cache[abilityName] = expectedPokemonNames;
            var pokemonService = new PokemonService(_mockHttpClient.Object);
            var result = pokemonService.GetPokemonByAbilityAsync(abilityName).Result;

            Assert.Equal(expectedPokemonNames.Count, result.Count);
            Assert.True(expectedPokemonNames.All(result.Contains));
            Assert.True(_cache.ContainsKey(abilityName));
            Assert.Equal(expectedPokemonNames, _cache[abilityName]);
        }


        [Fact]
        public void GetAllPokemonsByCache()
        {
            var abilityName = "levitate";
            var expectedPokemonNames = new List<string> { "gastly", "haunter", "koffing", "weezing", "misdreavus" };
            _cache[abilityName] = expectedPokemonNames;

            Assert.True(_cache.ContainsKey(abilityName));
            Assert.Equal(expectedPokemonNames, _cache[abilityName]);
        }

        [Fact]
        public void GetAllPokemonsByInvalidAbility()
        {
            var abilityName = "invalid_ability";
            var expectedPokemonNames = new List<string>();
            var _cache = new Dictionary<string, List<string>>();
            var pokemonService = new PokemonService(_mockHttpClient.Object);
            var result = pokemonService.GetPokemonByAbilityAsync(abilityName).Result;

            Assert.Equal(expectedPokemonNames.Count, result.Count);
            Assert.False(_cache.ContainsKey(abilityName));
        }
    }

}

