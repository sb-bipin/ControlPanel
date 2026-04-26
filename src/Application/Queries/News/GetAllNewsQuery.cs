using MediatR;
using Application.DTOs;

namespace Application.Queries;

public class GetAllNewsQuery : IRequest<List<NewsDto>>
{
}