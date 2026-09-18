using Application.Commands;
using Application.Common.Interfaces;
using Application.Common.Responses;  
using Application.DTOs;
using Application.Queries;
using FluentValidation;               
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class ImageGalleryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly string _storageRoot;

        public ImageGalleryController(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _storageRoot = configuration["ImageStorage:RootPath"]
                ?? throw new InvalidOperationException("ImageStorage:RootPath is not configured");
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<ServiceResponse<List<GalleryItemDto>>>> GetAll()
        {
            try
            {
                var items = await _mediator.Send(new GetAllGalleryItemsQuery());
                foreach (var item in items)
                {
                    item.ImageUrl = $"{Request.Scheme}://{Request.Host}{item.ImageUrl}";
                }
                return Ok(ServiceResponse<List<GalleryItemDto>>.SuccessResponse(
                    items, "Gallery items retrieved successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ServiceResponse<List<GalleryItemDto>>.ExceptionResponse(
                    ex, "Failed to retrieve gallery items."));
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<ServiceResponse<GalleryItemDto>>> Create([FromForm] CreateGalleryItemCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                result.ImageUrl = $"{Request.Scheme}://{Request.Host}{result.ImageUrl}";

                return CreatedAtAction(nameof(GetAll), new { id = result.Id },
                    ServiceResponse<GalleryItemDto>.SuccessResponse(result, "Image uploaded successfully.", StatusCodes.Status201Created));
            }
            catch (ValidationException ex)
            {
                return BadRequest(ServiceResponse<GalleryItemDto>.FailureResponse(
                    ex.Errors.Select(e => e.ErrorMessage).ToList(),
                    "Validation failed."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ServiceResponse<GalleryItemDto>.ExceptionResponse(
                    ex, "Failed to upload image."));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<object>>> Delete(Guid id)
        {
            try
            {
                await _mediator.Send(new DeleteGalleryItemCommand(id));
                return Ok(ServiceResponse<object>.SuccessResponse(null!, "Gallery item deleted successfully."));
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(ServiceResponse<object>.NotFoundResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ServiceResponse<object>.ExceptionResponse(
                    ex, "Failed to delete gallery item."));
            }
        }

        [HttpGet("image/{fileName}")]
        public IActionResult GetImage(string fileName)
        {
            var filePath = Path.Combine(_storageRoot, fileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileName, out var contentType))
                contentType = "application/octet-stream";

            var fileStream = System.IO.File.OpenRead(filePath);
            return File(fileStream, contentType);
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult<ServiceResponse<GalleryItemDto>>> Update(Guid id, [FromForm] UpdateGalleryItemCommand command)
        {
            if (id != command.Id)
                return BadRequest(ServiceResponse<GalleryItemDto>.FailureResponse("ID mismatch between URL and body."));

            var result = await _mediator.Send(command);
            result.ImageUrl = $"{Request.Scheme}://{Request.Host}{result.ImageUrl}";
            return Ok(ServiceResponse<GalleryItemDto>.SuccessResponse(result, "Image updated successfully."));
        }

        [HttpGet("getImageById/{id}")]
        public async Task<ActionResult<ServiceResponse<GalleryItemDto>>> GetById(Guid id)
        {
            var item = await _mediator.Send(new GetGalleryItemByIdQuery(id));
            item.ImageUrl = $"{Request.Scheme}://{Request.Host}{item.ImageUrl}";
            return Ok(ServiceResponse<GalleryItemDto>.SuccessResponse(item, "Gallery item retrieved."));
        }
    }
}