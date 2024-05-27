using Newtonsoft.Json;

namespace PokemonApi.Services
{
    public class PokemonService
    {
        private readonly HttpClient _httpClient;

        public PokemonService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
            var abilityData = JsonConvert.DeserializeObject<AbilityData>(response);

            var filteredPokemon = new List<string>();

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

            return filteredPokemon;
        }

        private int ExtractIdFromUrl(string url)
        {
            var segments = url.Split('/');
            return int.Parse(segments[segments.Length - 2]);
        }
    }
}

