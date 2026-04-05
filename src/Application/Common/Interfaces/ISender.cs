using Application.Common.Abstractions;

namespace Application.Common.Interfaces;

public interface ISender
{
    Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken ct = default);
    Task<TResponse> Send<TResponse>(IQuery<TResponse> query, CancellationToken ct = default);
}
