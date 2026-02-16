using AgroSolutions.Property.API.CustomAttributes;
using AgroSolutions.Property.API.Extensions;
using AgroSolutions.Property.API.InputModels;
using AgroSolutions.Property.API.Responses;
using AgroSolutions.Property.Application.Commands.CreateProperty;
using AgroSolutions.Property.Application.Commands.CreatePropertyFields;
using AgroSolutions.Property.Application.Commands.DeleteProperty;
using AgroSolutions.Property.Application.Commands.DeletePropertyField;
using AgroSolutions.Property.Application.Commands.UpdateProperty;
using AgroSolutions.Property.Application.Commands.UpdatePropertyField;
using AgroSolutions.Property.Application.Queries.GetProperties;
using AgroSolutions.Property.Application.Queries.GetProperty;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace AgroSolutions.Property.API.Controllers.v1;

[Authorize, ValidateToken]
[ApiController]
[Route("api/v1/properties")]
public class PropertiesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(EmptyResult))]
    public async Task<OkObjectResult> GetProperties()
    {
        Log.Information("Starting Action {ActionName}.", nameof(GetProperties));
        GetPropertiesQuery query = new(User.UserId);
        IEnumerable<GetPropertiesQueryResult>? getPropertiesQueryResults = await _mediator.Send(query);
        return Ok(getPropertiesQueryResults);
    }

    [HttpGet("{propertyId}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(EmptyResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(RestResponse))]
    public async Task<OkObjectResult> GetProperty(int propertyId)
    {
        Log.Information("Starting Action {ActionName}.", nameof(GetProperty));
        GetPropertyQuery query = new(User.UserId, propertyId);
        GetPropertyQueryResult? getPropertyQueryResult = await _mediator.Send(query);
        return Ok(getPropertyQueryResult);
    }

    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RestResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(RestResponseWithInvalidFields))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(EmptyResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(RestResponse))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(RestResponse))]
    public async Task<CreatedAtActionResult> CreateProperty(CreatePropertyInputModel inputModel)
    {
        Log.Information("Starting Action {ActionName}.", nameof(CreateProperty));
        CreatePropertyCommand command = new(User.UserId, inputModel.Name, inputModel.Description);
        CreatePropertyCommandResult? createPropertyCommandResult = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetProperty), new { propertyId = createPropertyCommandResult?.PropertyId }, createPropertyCommandResult);
    }

    [HttpPatch("{propertyId}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(RestResponseWithInvalidFields))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(EmptyResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(RestResponse))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(RestResponse))]
    public async Task<OkObjectResult> UpdateProperty(int propertyId, UpdatePropertyInputModel inputModel)
    {
        Log.Information("Starting Action {ActionName}.", nameof(UpdateProperty));
        UpdatePropertyCommand command = new(User.UserId, propertyId, inputModel.Name, inputModel.Description);
        UpdatePropertyCommandResult? updatePropertyCommandResult = await _mediator.Send(command);
        return Ok(updatePropertyCommandResult);
    }

    [HttpDelete("{propertyId}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(EmptyResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(RestResponse))]
    public async Task<NoContentResult> DeleteProperty(int propertyId)
    {
        Log.Information("Starting Action {ActionName}.", nameof(DeleteProperty));
        DeletePropertyCommand command = new(User.UserId, propertyId);
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("{propertyId}/fields")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(RestResponseWithInvalidFields))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(EmptyResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(RestResponse))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(RestResponse))]
    public async Task<OkObjectResult> CreatePropertyFields(int propertyId, [FromBody] IEnumerable<CreatePropertyFieldInputModel> fields)
    {
        Log.Information("Starting Action {ActionName}.", nameof(CreatePropertyFields));
        CreatePropertyFieldsCommand command = new(User.UserId, propertyId, fields.Select(f => new CreatePropertyFieldData(f.Name, f.CropId, f.TotalAreaInHectares)));
        IEnumerable<CreatePropertyFieldsCommandResult>? createPropertyFieldsCommandResults = await _mediator.Send(command);
        return Ok(createPropertyFieldsCommandResults);
    }

    [HttpPatch("{propertyId}/fields/{fieldId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(RestResponseWithInvalidFields))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(EmptyResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(RestResponse))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(RestResponse))]
    public async Task<OkObjectResult> UpdatePropertyField(int propertyId, int fieldId, UpdateFieldInputModel inputModel)
    {
        Log.Information("Starting Action {ActionName}.", nameof(UpdatePropertyField));
        UpdatePropertyFieldCommand command = new(User.UserId, propertyId, fieldId, inputModel.Name, inputModel.CropId, inputModel.TotalAreaInHectares);
        UpdatePropertyFieldCommandResult? updatePropertyFieldCommandResult = await _mediator.Send(command);
        return Ok(updatePropertyFieldCommandResult);
    }

    [HttpDelete("{propertyId}/fields/{fieldId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(EmptyResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(RestResponse))]
    public async Task<NoContentResult> DeletePropertyField(int propertyId, int fieldId)
    {
        Log.Information("Starting Action {ActionName}.", nameof(DeletePropertyField));
        DeletePropertyFieldCommand command = new(User.UserId, propertyId, fieldId);
        await _mediator.Send(command);
        return NoContent();
    }
}
