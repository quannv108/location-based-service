namespace LBS.Contracts.Dto;

public class GetNearbyLocationsInputDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public float Radius { get; set; }
}