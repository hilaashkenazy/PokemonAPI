using Newtonsoft.Json;

namespace PokemonAPI.Models
{
    public class Pokemon
    {
        [JsonProperty("name")]
        public required string Name { get; set; }

        [JsonProperty("url")]
        public required string Url { get; set; }
    }
}
