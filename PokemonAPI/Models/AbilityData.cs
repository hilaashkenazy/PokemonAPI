using Newtonsoft.Json;

namespace PokemonAPI.Models
{
    public class AbilityData
    {
        [JsonProperty("pokemon")]
        public required List<PokemonAbility> Pokemon { get; set; }
    }
}
