using System.ComponentModel.DataAnnotations;

namespace LBS.Contracts.Dto;

public class CreateNamedLocationInputDto
{
    [Required]
    public string Name { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
}