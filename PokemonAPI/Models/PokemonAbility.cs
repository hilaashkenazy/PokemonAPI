using Newtonsoft.Json;

namespace PokemonAPI.Models
{
    public class PokemonAbility
    {
        [JsonProperty("pokemon")]
        public required Pokemon Pokemon { get; set; }
    }
}
