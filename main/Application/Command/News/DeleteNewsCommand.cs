using MediatR;

namespace Application.Commands;

public class DeleteNewsCommand : IRequest
{
    public Guid Id { get; set; }
}