using Application.Commands;
using Application.Common.Responses;
using Application.DTOs;
using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NewsController : ControllerBase
{
    private readonly IMediator _mediator;

    public NewsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all news items (ordered by publish date descending)
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<NewsDto>>>> GetAll()
    {
        try
        {
            var items = await _mediator.Send(new GetAllNewsQuery());

            foreach (var item in items)
            {
                if (!string.IsNullOrEmpty(item.ImageUrl) && !item.ImageUrl.StartsWith("http"))
                {
                    item.ImageUrl = $"{Request.Scheme}://{Request.Host}{item.ImageUrl}";
                }
            }

            return Ok(ServiceResponse<List<NewsDto>>.SuccessResponse(
                items, "News retrieved successfully."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ServiceResponse<List<NewsDto>>.ExceptionResponse(
                ex, "Failed to retrieve news."));
        }
    }

    /// <summary>
    /// Get a single news item by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceResponse<NewsDto>>> GetById(Guid id)
    {
        try
        {
            var result = await _mediator.Send(new GetNewsByIdQuery { Id = id });
            if (result == null)
            {
                return NotFound(ServiceResponse<NewsDto>.FailureResponse("News not found."));
            }

            if (!string.IsNullOrEmpty(result.ImageUrl) && !result.ImageUrl.StartsWith("http"))
            {
                result.ImageUrl = $"{Request.Scheme}://{Request.Host}{result.ImageUrl}";
            }

            return Ok(ServiceResponse<NewsDto>.SuccessResponse(result, "News retrieved."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ServiceResponse<NewsDto>.ExceptionResponse(
                ex, "Failed to retrieve news."));
        }
    }

    /// <summary>
    /// Create a new news item (with optional image upload)
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<NewsDto>>> Create([FromForm] CreateNewsCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            var response = ServiceResponse<NewsDto>.SuccessResponse(result, "News created successfully.");
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ServiceResponse<NewsDto>.ExceptionResponse(
                ex, "Failed to create news."));
        }
    }

    /// <summary>
    /// Update an existing news item (with optional image upload)
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ServiceResponse<NewsDto>>> Update(Guid id, [FromForm] UpdateNewsCommand command)
    {
        try
        {
            if (id != command.Id)
            {
                return BadRequest(ServiceResponse<NewsDto>.FailureResponse("ID mismatch."));
            }

            var result = await _mediator.Send(command);
            if (result == null)
            {
                return NotFound(ServiceResponse<NewsDto>.FailureResponse("News not found."));
            }

            return Ok(ServiceResponse<NewsDto>.SuccessResponse(result, "News updated successfully."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ServiceResponse<NewsDto>.ExceptionResponse(
                ex, "Failed to update news."));
        }
    }

    /// <summary>
    /// Delete a news item (also removes associated image file)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ServiceResponse<object>>> Delete(Guid id)
    {
        try
        {
            await _mediator.Send(new DeleteNewsCommand { Id = id });
            return Ok(ServiceResponse<object>.SuccessResponse(null!, "News deleted successfully."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ServiceResponse<object>.ExceptionResponse(
                ex, "Failed to delete news."));
        }
    }
}