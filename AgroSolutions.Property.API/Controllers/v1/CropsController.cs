using AgroSolutions.Property.API.Responses;
using AgroSolutions.Property.Application.Queries.GetCrops;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace AgroSolutions.Property.API.Controllers.v1;

[ApiController]
[Route("api/v1/crops")]
public class CropsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestResponse))]
    public async Task<OkObjectResult> GetCrops()
    {
        Log.Information("Starting Action {ActionName}.", nameof(GetCrops));
        GetCropsQuery query = new();
        IEnumerable<GetCropQueryResult> getCropQueryResults = await _mediator.Send(query);
        return Ok(getCropQueryResults);
    }
}
