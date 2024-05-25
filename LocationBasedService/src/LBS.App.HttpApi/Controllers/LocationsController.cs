using System.Net;
using LBS.Contracts;
using LBS.Contracts.Dto;
using Microsoft.AspNetCore.Mvc;

namespace LBS.App.HttpApi.Controllers;

[Area("lbs")]
[Route("api/[area]/[controller]")]
public class LocationsController : Controller
{
    private readonly INamedLocationService _namedLocationService;

    public LocationsController(INamedLocationService namedLocationService)
    {
        _namedLocationService = namedLocationService;
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateNamedLocationInputDto input)
    {
        await _namedLocationService.CreateAsync(input);
        return Created();
    }
    
    [HttpGet("nearest")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetNearest(GetNearbyLocationsInputDto input)
    {
        var locations = await _namedLocationService.GetNearbyLocationAsync(input);
        return Ok(locations);
    }
}