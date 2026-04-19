using MediatR;
using Application.DTOs;

namespace Application.Queries;

public class GetAllNoticesQuery : IRequest<List<NoticeDto>>
{
}