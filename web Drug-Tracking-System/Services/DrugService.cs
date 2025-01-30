using MongoDB.Driver;

public class DrugService
{
    private readonly IMongoCollection<Drug> _drugCollection;

    public DrugService(MongoDbContext context)
    {
        _drugCollection = context.Drugs;
    }

    public async Task<List<Drug>> GetDrugsAsync() => await _drugCollection.Find(_ => true).ToListAsync();

    public async Task<List<Drug>> SearchDrugsAsync(string name) =>
        await _drugCollection.Find(d => d.Name.ToLower().Contains(name.ToLower())).ToListAsync();

    public async Task CreateDrugAsync(Drug drug) => await _drugCollection.InsertOneAsync(drug);

    public async Task<bool> DeleteDrugAsync(string id)
    {
        var result = await _drugCollection.DeleteOneAsync(d => d.Id == id);
        return result.DeletedCount > 0;
    }

}