using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Pokedex.Services;

namespace Pokedex.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IMongoCollection<BsonDocument> _pokemonCollection;

        public PokemonController(MongoDbService mongoDbService)
        {
            _pokemonCollection = mongoDbService.GetCollection<BsonDocument>("Pokemon");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPokemon()
        {
            var pokemonList = await _pokemonCollection.Find(new BsonDocument()).ToListAsync();
            return Ok(pokemonList);
        }

        [HttpPost]
        public async Task<IActionResult> AddPokemon([FromBody] BsonDocument pokemon)
        {
            await _pokemonCollection.InsertOneAsync(pokemon);
            return Created("", pokemon);
        }
    }
}
