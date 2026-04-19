using Application.Commands;
using Application.Common.Responses;
using Application.DTOs;
using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly string _storageRoot;

    public EventsController(IMediator mediator, IConfiguration configuration)
    {
        _mediator = mediator;
        _storageRoot = configuration["ImageStorage:RootPath"]
            ?? throw new InvalidOperationException("ImageStorage:RootPath is not configured");
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<EventDto>>>> GetAll()
    {
        try
        {
            var items = await _mediator.Send(new GetAllEventsQuery());
            foreach (var item in items)
            {
                item.ImageUrl = $"{Request.Scheme}://{Request.Host}{item.ImageUrl}";
            }
            return Ok(ServiceResponse<List<EventDto>>.SuccessResponse(
                items, "Events retrieved successfully."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ServiceResponse<List<EventDto>>.ExceptionResponse(
                ex, "Failed to retrieve events."));
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceResponse<EventDto>>> GetById(Guid id)
    {
        try
        {
            var result = await _mediator.Send(new GetEventByIdQuery(id));
            if (result == null)
            {
                return NotFound(ServiceResponse<EventDto>.FailureResponse("Event not found."));
            }
            return Ok(ServiceResponse<EventDto>.SuccessResponse(result, "Event retrieved."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ServiceResponse<EventDto>.ExceptionResponse(
                ex, "Failed to retrieve event."));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<EventDto>>> Create([FromForm] CreateEventCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            var response = ServiceResponse<EventDto>.SuccessResponse(result, "Event created.");
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ServiceResponse<EventDto>.ExceptionResponse(
                ex, "Failed to create event."));
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ServiceResponse<EventDto>>> Update(Guid id, [FromForm] UpdateEventCommand command)
    {
        try
        {
            if (id != command.Id)
            {
                return BadRequest(ServiceResponse<EventDto>.FailureResponse(
                    "ID mismatch."));
            }

            var result = await _mediator.Send(command);
            if (result == null)
            {
                return NotFound(ServiceResponse<EventDto>.FailureResponse(
                    "Event not found."));
            }

            return Ok(ServiceResponse<EventDto>.SuccessResponse(result, "Event updated."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ServiceResponse<EventDto>.ExceptionResponse(
                ex, "Failed to update event."));
        }
    }

    [HttpDelete("{id}")]    
    public async Task<ActionResult<ServiceResponse<Object>>> Delete(Guid id)
    {
        try
        {
            await _mediator.Send(new DeleteEventCommand(id));
            return Ok(ServiceResponse<Object>.SuccessResponse("Event deleted successfully."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ServiceResponse<Object>.ExceptionResponse(
                ex, "Failed to delete event."));
        }
    }
}