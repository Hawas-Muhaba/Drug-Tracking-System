using MongoDB.Driver;
using DrugSystem.Models;
using DrugSystem.DTOs;
using DrugSystem.Data;

namespace DrugSystem.Services
{
    public class PharmacyService
    {
        private readonly IMongoCollection<Pharmacy> _pharmacyCollection;

        public PharmacyService(MongoDbContext context)
        {
            _pharmacyCollection = context.Pharmacies;
        }

        public async Task<Pharmacy> CreatePharmacyAsync(CreatePharmacyDto dto)
        {
            var pharmacy = new Pharmacy
            {
                Name = dto.Name,
                Location = dto.Location
            };

            await _pharmacyCollection.InsertOneAsync(pharmacy);
            return pharmacy; 
        }

        public async Task<PharmacyResponseDto> EditPharmacyAsync(string id, EditPharmacyDto pharmacyDto)
        {
            var pharmacy = await _pharmacyCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
            if (pharmacy == null)
            {
                throw new KeyNotFoundException("Pharmacy not found.");
            }

            pharmacy.Name = pharmacyDto.Name;
            pharmacy.Location = new Location
            {
                Latitude = pharmacyDto.Location.Latitude,
                Longitude = pharmacyDto.Location.Longitude
            };

            await _pharmacyCollection.ReplaceOneAsync(p => p.Id == id, pharmacy);

            return new PharmacyResponseDto
            {
                Id = pharmacy.Id,
                Name = pharmacy.Name,
                Location = new Location
                {
                    Latitude = pharmacy.Location.Latitude,
                    Longitude = pharmacy.Location.Longitude
                }
            };
        }

        public async Task<Pharmacy?> GetPharmacyByIdAsync(string id) =>
            await _pharmacyCollection.Find(p => p.Id == id).FirstOrDefaultAsync();

        public async Task<List<Pharmacy>> GetAllPharmaciesAsync(int pageNumber = 1, int pageSize = 10)
        {
            int skip = (pageNumber - 1) * pageSize;

            var totalCount = await _pharmacyCollection.CountDocumentsAsync(FilterDefinition<Pharmacy>.Empty);

            var pharmacies = await _pharmacyCollection
            .Find(_ => true)
            .Skip(skip)
            .Limit(pageSize)
            .ToListAsync();

            return pharmacies;
        }
       

        public async Task<bool> DeletePharmacyAsync(string id)
        {
            var result = await _pharmacyCollection.DeleteOneAsync(p => p.Id == id);
            return result.DeletedCount > 0;
        }

        
    }
}
