using Microsoft.AspNetCore.Http;

namespace Application.Common.Interfaces
{
    public interface IExceptionToResponseMapper
    {
        Task HandleExceptionAsync(HttpContext context, Exception exception);
    }

}