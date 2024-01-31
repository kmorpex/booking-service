using GoVilla.Domain.Abstractions;
using MediatR;

namespace GoVilla.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}