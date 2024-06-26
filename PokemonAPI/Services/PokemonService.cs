﻿using Newtonsoft.Json;
using PokemonAPI.Models;

namespace PokemonAPI.Services
{
    public class PokemonService
    {
        private readonly HttpClient _httpClient;
        private readonly Dictionary<string,List<string>> _cache;

        public PokemonService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _cache = [];

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
                throw;
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
            
            // Add pokemons that have the ability to list.
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

        // Extract ID of pokemon from url to make sure it is part of the first 200 pokemons.
        private int ExtractIdFromUrl(string url)
        {
            var segments = url.Split('/');
            return int.Parse(segments[^2]);
        }
    }
}

