
public class Drug
{
     public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public required string PharmacyId { get; set; }
    public required double Price { get; set; }
    public required int Quantity { get; set; }
}
