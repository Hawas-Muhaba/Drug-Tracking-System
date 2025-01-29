using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DrugService
{
    private readonly IMongoCollection<Drug> _drugs;

    public DrugService(IMongoDatabase database)
    {
        _drugs = database.GetCollection<Drug>("Drugs");
    }

    public async Task<List<Drug>> GetAllAsync() => await _drugs.Find(d => true).ToListAsync();

    public async Task<Drug> GetByIdAsync(string id) => await _drugs.Find(d => d.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Drug drug) => await _drugs.InsertOneAsync(drug);

    public async Task UpdateAsync(string id, Drug updatedDrug) =>
        await _drugs.ReplaceOneAsync(d => d.Id == id, updatedDrug);

    public async Task DeleteAsync(string id) => await _drugs.DeleteOneAsync(d => d.Id == id);
}