using MongoDB.Driver;

public class PharmacyService
{
    private readonly IMongoCollection<Pharmacy> _pharmacyCollection;

    public PharmacyService(MongoDbContext context)
    {
        _pharmacyCollection = context.Pharmacies;
    }

    public async Task CreatePharmacyAsync(Pharmacy pharmacy) => await _pharmacyCollection.InsertOneAsync(pharmacy);

    public async Task<Pharmacy> GetPharmacyByIdAsync(string id) =>
        await _pharmacyCollection.Find(p => p.Id == id).FirstOrDefaultAsync();

    public async Task<List<Pharmacy>> GetAllPharmaciesAsync() => await _pharmacyCollection.Find(_ => true).ToListAsync();

    public async Task<bool> DeletePharmacyAsync(string id)
    {
        var result = await _pharmacyCollection.DeleteOneAsync(p => p.Id == id);
        return result.DeletedCount > 0;
    }
}