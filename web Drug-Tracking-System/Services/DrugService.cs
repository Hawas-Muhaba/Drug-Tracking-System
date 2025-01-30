using MongoDB.Driver;
using DrugSystem.Data;
using DrugSystem.Models;
using DrugSystem.DTOs;
using MongoDB.Bson;

namespace DrugSystem.Services
{
    public class DrugService
    {
        private readonly IMongoCollection<Drug> _drugCollection;
        private readonly IMongoCollection<Pharmacy> _pharmacyCollection;

        public DrugService(MongoDbContext context)
        {
            _drugCollection = context.Drugs;
            _pharmacyCollection = context.Pharmacies;
        }

        public async Task<List<DrugResponseDto>> GetDrugsAsync(int pageNumber = 1, int pageSize = 10)
        {
            int skip = (pageNumber - 1) * pageSize;

            var totalCount = await _drugCollection.CountDocumentsAsync(FilterDefinition<Drug>.Empty);

            var drugs = await _drugCollection
            .Find(_ => true)
            .Skip(skip)
            .Limit(pageSize)
            .ToListAsync();
            var drugDtos = new List<DrugResponseDto>();

            foreach (var drug in drugs)
            {
                var pharmacy = await _pharmacyCollection
                    .Find(p => p.Id == drug.PharmacyId)
                    .FirstOrDefaultAsync();

                var pharmacyName = pharmacy?.Name ?? "Unknown Pharmacy";

                drugDtos.Add(new DrugResponseDto
                {
                    Id = drug.Id.ToString(),
                    Name = drug.Name,
                    PharmacyName = pharmacyName,
                    Price = drug.Price,
                    Quantity = drug.Quantity
                });
            }

            return drugDtos;
        }

        public async Task<List<DrugResponseDto>> SearchDrugsAsync(string name)
        {
            var drugs = await _drugCollection
                .Find(d => d.Name.ToLower().Contains(name.ToLower()))
                .ToListAsync();
            var drugDtos = new List<DrugResponseDto>();

            foreach (var drug in drugs)
            {
                if (drug == null)
                {
                    continue;
                }
                var pharmacy = await _pharmacyCollection
                    .Find(p => p.Id == drug.PharmacyId)
                    .FirstOrDefaultAsync();

                var pharmacyName = pharmacy?.Name ?? "Unknown Pharmacy";

                drugDtos.Add(new DrugResponseDto
                {
                    Id = drug.Id.ToString(),
                    Name = drug.Name,
                    PharmacyName = pharmacyName,
                    Price = drug.Price,
                    Quantity = drug.Quantity
                });
            }


            return drugDtos;
        }

        public async Task<DrugResponseDto> CreateDrugAsync(CreateDrugDto drugDto)
        {
            var pharmacyExists = await CheckPharmacyExistsAsync(drugDto.PharmacyId);
            if (!pharmacyExists)
            {
                throw new ArgumentException("The pharmacy ID provided is invalid or does not exist.");
            }

            var existingDrug = await _drugCollection
                .Find(d => d.Name.ToLower() == drugDto.Name.ToLower() && d.PharmacyId == drugDto.PharmacyId)
                .FirstOrDefaultAsync();

            if (existingDrug != null)
            {
                throw new ArgumentException($"A drug with the name '{drugDto.Name}' already exists in this pharmacy.");
            }

            var drug = new Drug
            {
                Name = drugDto.Name,
                PharmacyId = drugDto.PharmacyId,
                Price = drugDto.Price,
                Quantity = drugDto.Quantity
            };

             await _drugCollection.InsertOneAsync(drug);
            
            var pharmacy = await _pharmacyCollection
                    .Find(p => p.Id == drugDto.PharmacyId)
                    .FirstOrDefaultAsync();

            var pharmacyName = pharmacy?.Name ?? "Unknown Pharmacy";

            return new DrugResponseDto
            {
                Id = drug.Id.ToString(),
                Name = drug.Name,
                PharmacyName = pharmacyName,
                Price = drug.Price,
                Quantity = drug.Quantity
            };
        }

        public async Task<DrugResponseDto> EditDrugAsync(string id, EditDrugDto drugDto)
        {
            var drug = await _drugCollection.Find(d => d.Id.ToString() == id).FirstOrDefaultAsync();
            if (drug == null)
            {
                throw new KeyNotFoundException("Drug not found.");
            }

            var pharmacyExists = await CheckPharmacyExistsAsync(drugDto.PharmacyId);
            if (!pharmacyExists)
            {
                throw new ArgumentException("The pharmacy ID provided is invalid or does not exist.");
            }

            drug.Name = drugDto.Name;
            drug.Price = (double)drugDto.Price;
            drug.Quantity = drugDto.Quantity;
            drug.PharmacyId = drugDto.PharmacyId;

            await _drugCollection.ReplaceOneAsync(d => d.Id.ToString() == id, drug);

            var pharmacy = await _pharmacyCollection
                .Find(p => p.Id == drug.PharmacyId)
                .FirstOrDefaultAsync();

            var pharmacyName = pharmacy?.Name ?? "Unknown Pharmacy";

            return new DrugResponseDto
            {
                Id = drug.Id.ToString(),
                Name = drug.Name,
                PharmacyName = pharmacyName,
                Price = drug.Price,
                Quantity = drug.Quantity
            };
        }


        public async Task<bool> DeleteDrugAsync(string id)
        {
            var result = await _drugCollection.DeleteOneAsync(d => d.Id.ToString() == id);
            return result.DeletedCount > 0;
        }

        public async Task<DrugResponseDto?> GetDrugByIdAsync(string id)
        {
            var drug = await _drugCollection.Find(d => d.Id.ToString() == id).FirstOrDefaultAsync();

            if (drug == null)
            {
                return null; 
            }

            var pharmacy = await _pharmacyCollection
                .Find(p => p.Id == drug.PharmacyId)
                .FirstOrDefaultAsync();

            var pharmacyName = pharmacy?.Name ?? "Unknown Pharmacy";

            return new DrugResponseDto
            {
                Id = drug.Id.ToString(),
                Name = drug.Name,
                PharmacyName = pharmacyName,
                Price = drug.Price,
                Quantity = drug.Quantity
            };
        }

        public async Task<bool> CheckPharmacyExistsAsync(string pharmacyId)
        {
            if (!ObjectId.TryParse(pharmacyId, out var parsedId))
            {
                return false;
            }

            var pharmacy = await _pharmacyCollection
                .Find(p => p.Id == parsedId.ToString()) 
                .FirstOrDefaultAsync();

            return pharmacy != null; 
        }
    }
}
