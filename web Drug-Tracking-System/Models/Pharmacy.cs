
public class Pharmacy
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public required Location Location { get; set; }
}
