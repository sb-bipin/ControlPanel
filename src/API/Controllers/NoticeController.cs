using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Commands;
using Application.Queries;
using Application.Common.Responses;
using Application.DTOs;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NoticesController : ControllerBase
{
    private readonly IMediator _mediator;

    public NoticesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<NoticeDto>>>> GetAll()
    {
        try
        {
            var items = await _mediator.Send(new GetAllNoticesQuery());
            foreach (var item in items)
            {
                if (!string.IsNullOrEmpty(item.ImageUrl) && !item.ImageUrl.StartsWith("http"))
                {
                    item.ImageUrl = $"{Request.Scheme}://{Request.Host}{item.ImageUrl}";
                }
            }
            return Ok(ServiceResponse<List<NoticeDto>>.SuccessResponse(items, "Notices retrieved successfully."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ServiceResponse<List<NoticeDto>>.ExceptionResponse(ex, "Failed to retrieve notices."));
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceResponse<NoticeDto>>> GetById(Guid id)
    {
        try
        {
            var result = await _mediator.Send(new GetNoticeByIdQuery { Id = id });
            if (result == null)
                return NotFound(ServiceResponse<NoticeDto>.FailureResponse("Notice not found."));

            if (!string.IsNullOrEmpty(result.ImageUrl) && !result.ImageUrl.StartsWith("http"))
                result.ImageUrl = $"{Request.Scheme}://{Request.Host}{result.ImageUrl}";

            return Ok(ServiceResponse<NoticeDto>.SuccessResponse(result, "Notice retrieved."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ServiceResponse<NoticeDto>.ExceptionResponse(ex, "Failed to retrieve notice."));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<NoticeDto>>> Create([FromForm] CreateNoticeCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id },
                ServiceResponse<NoticeDto>.SuccessResponse(result, "Notice created successfully."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ServiceResponse<NoticeDto>.ExceptionResponse(ex, "Failed to create notice."));
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ServiceResponse<NoticeDto>>> Update(Guid id, [FromForm] UpdateNoticeCommand command)
    {
        try
        {
            if (id != command.Id)
                return BadRequest(ServiceResponse<NoticeDto>.FailureResponse("ID mismatch."));

            var result = await _mediator.Send(command);
            if (result == null)
                return NotFound(ServiceResponse<NoticeDto>.FailureResponse("Notice not found."));

            return Ok(ServiceResponse<NoticeDto>.SuccessResponse(result, "Notice updated successfully."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ServiceResponse<NoticeDto>.ExceptionResponse(ex, "Failed to update notice."));
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ServiceResponse<object>>> Delete(Guid id)
    {
        try
        {
            await _mediator.Send(new DeleteNoticeCommand { Id = id });
            return Ok(ServiceResponse<object>.SuccessResponse(null, "Notice deleted successfully."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ServiceResponse<object>.ExceptionResponse(ex, "Failed to delete notice."));
        }
    }
}