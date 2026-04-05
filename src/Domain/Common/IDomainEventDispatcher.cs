namespace Domain.Common;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IReadOnlyList<IDomainEvent> domainEvents, CancellationToken ct = default);
}
