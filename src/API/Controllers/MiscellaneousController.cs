using Application.Common.Responses;
using Application.DTOs;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MiscellaneousController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MiscellaneousController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /*
         Submission of Admission Form:
         * */
        //[HttpPost("admissionForm")]
        //public async Task<IActionResult> SubmitAdmissionForm([FromBody] SubmitAdmissionFormCommand command)
        //{
        //    try
        //    {
        //        var result = await _mediator.Send(command);
        //        result.ImageUrl = $"{Request.Scheme}://{Request.Host}{result.ImageUrl}";

        //        return CreatedAtAction(nameof(SubmitAdmissionForm), new { id = result.Id },
        //            ServiceResponse<GalleryItemDto>.SuccessResponse(result,
        //                        "Admission form submitted successfully.", StatusCodes.Status201Created));
        //    }
        //    catch (ValidationException ex)
        //    {
        //        return BadRequest(ServiceResponse<GalleryItemDto>.FailureResponse(
        //            ex.Errors.Select(e => e.ErrorMessage).ToList(),
        //            "Validation failed."));
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ServiceResponse<GalleryItemDto>.ExceptionResponse(
        //            ex, "Failed to submit admission form."));
        //    }
        //}

    }
}
