using MediatR;

namespace Application.Commands;

public class DeleteNoticeCommand : IRequest
{
    public Guid Id { get; set; }
}