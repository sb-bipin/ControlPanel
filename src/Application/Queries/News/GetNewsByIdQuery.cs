using MediatR;
using Application.DTOs;

namespace Application.Queries;

public class GetNewsByIdQuery : IRequest<NewsDto?>
{
    public Guid Id { get; set; }
}