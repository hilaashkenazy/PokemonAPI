using Microsoft.AspNetCore.Mvc;
using PokemonAPI.Models;
using PokemonAPI.Services;

namespace PokemonAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly PokemonService _pokemonService;

        public PokemonController(PokemonService pokemonService)
        {
            _pokemonService = pokemonService;
        }

           [HttpPost]
        public async Task<IActionResult> GetPokemonByAbilities([FromBody] AbilityRequest abilityRequest)
        {
            if (abilityRequest?.Abilities == null || abilityRequest.Abilities.Count == 0)
            {
                return BadRequest("Abilities list is empty or null.");
            }
            var uniqueNames = new HashSet<string>();

            foreach (var ability in abilityRequest.Abilities)
            {
                var pokemonWithAbility = await _pokemonService.GetPokemonByAbilityAsync(ability);
              //  allFilteredPokemon.AddRange(pokemonWithAbility);
                foreach (var pokemon in pokemonWithAbility)
                {
                    uniqueNames.Add(pokemon); 
                }
            }
        return Ok(uniqueNames);
        }
    }
}