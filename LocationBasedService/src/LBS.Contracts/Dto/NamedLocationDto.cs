namespace LBS.Contracts.Dto;

public class NamedLocationDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}