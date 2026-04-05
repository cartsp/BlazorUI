using Application.Common.Abstractions;
using Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Mediator;

public sealed class Sender(IServiceProvider serviceProvider) : ISender
{
    public async Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken ct = default)
    {
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResponse));
        var handler = serviceProvider.GetRequiredService(handlerType);
        var method = handlerType.GetMethod("Handle");
        return await (Task<TResponse>)method!.Invoke(handler, [command, ct])!;
    }

    public async Task<TResponse> Send<TResponse>(IQuery<TResponse> query, CancellationToken ct = default)
    {
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponse));
        var handler = serviceProvider.GetRequiredService(handlerType);
        var method = handlerType.GetMethod("Handle");
        return await (Task<TResponse>)method!.Invoke(handler, [query, ct])!;
    }
}
