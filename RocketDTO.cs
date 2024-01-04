public class RocketDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsLaunched { get; set; }
    public RocketDTO() {}
    public RocketDTO(Rocket rocketItem) =>
    (Id, Name, IsLaunched) = (rocketItem.Id, rocketItem.Name, rocketItem.IsLaunched);
}