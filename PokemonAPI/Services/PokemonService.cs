using Newtonsoft.Json;
using PokemonAPI.Models;
using Microsoft.Extensions.Caching.Memory;

namespace PokemonApi.Services
{
    public class PokemonService
    {
        private readonly HttpClient _httpClient;
        private readonly Dictionary<string,List<string>> _cache;

        public PokemonService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _cache = new Dictionary<string, List<string>>();

        }

         public async Task<List<string>> GetPokemonByAbilityAsync(string ability)
        {
            var response = string.Empty;

            try {

                response = await _httpClient.GetStringAsync($"https://pokeapi.co/api/v2/ability/{ability}");

            }
            catch (HttpRequestException e) // Catches errors related to the request
            {
            Console.WriteLine($"Error retrieving data: {e.Message}");
            }

            // first check if ability is in dictionary else send request 
            if (_cache.ContainsKey(ability)) 
            { 
                return _cache[ability];
            }
            

            var filteredPokemon = new List<string>();


            var abilityData = JsonConvert.DeserializeObject<AbilityData>(response);

            

            if (abilityData == null || abilityData.Pokemon == null)
            {
                return filteredPokemon; // Return an empty list to indicate no data
            }

            foreach (var pokemonEntry in abilityData.Pokemon)
            {
                var pokemonId = ExtractIdFromUrl(pokemonEntry.Pokemon.Url);
                if (pokemonId > 200)
                {
                    break;
                }
                filteredPokemon.Add(pokemonEntry.Pokemon.Name);
            }

            // Add new call to dictionary
            _cache[ability] = filteredPokemon;
            

            return filteredPokemon;
        }

        private int ExtractIdFromUrl(string url)
        {
            var segments = url.Split('/');
            return int.Parse(segments[segments.Length - 2]);
        }
    }
}

