using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using NetTopologySuite.Geometries;

namespace LBS.Dal.Models;

[Table("NamedLocations", Schema = "lbs")]
public class NamedLocation
{
    [Key] public Guid Id { get; set; }

    [NotNull] [MaxLength(128)]
    public string Name { get; set; }
    
    [Column(TypeName = "geometry (point)")]
    public Point Location { get; set; }
    
    [MaxLength(16)]
    public string GeoHash { get; set; }
}