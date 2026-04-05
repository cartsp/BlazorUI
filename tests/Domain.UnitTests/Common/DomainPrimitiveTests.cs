using AwesomeAssertions;

namespace Domain.UnitTests.Common;

public class ValueObjectTests
{
    [Fact]
    public void ValueObjects_WithSameComponents_ShouldBeEqual()
    {
        // Arrange
        var vo1 = new TestValueObject("test", 42);
        var vo2 = new TestValueObject("test", 42);

        // Act & Assert
        vo1.Should().Be(vo2);
        vo1.GetHashCode().Should().Be(vo2.GetHashCode());
    }

    [Fact]
    public void ValueObjects_WithDifferentComponents_ShouldNotBeEqual()
    {
        // Arrange
        var vo1 = new TestValueObject("test", 42);
        var vo2 = new TestValueObject("test", 99);

        // Act & Assert
        vo1.Should().NotBe(vo2);
    }

    private sealed class TestValueObject(string name, int value) : Domain.Common.ValueObject
    {
        public string Name { get; } = name;
        public int Value { get; } = value;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return Value;
        }
    }
}

public class EntityTests
{
    [Fact]
    public void Entities_WithSameId_ShouldBeEqual()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity1 = new TestEntity(id);
        var entity2 = new TestEntity(id);

        // Act & Assert
        entity1.Should().Be(entity2);
    }

    [Fact]
    public void Entities_WithDifferentId_ShouldNotBeEqual()
    {
        // Arrange
        var entity1 = new TestEntity(Guid.NewGuid());
        var entity2 = new TestEntity(Guid.NewGuid());

        // Act & Assert
        entity1.Should().NotBe(entity2);
    }

    [Fact]
    public void AddDomainEvent_ShouldTrackEvent()
    {
        // Arrange
        var entity = new TestEntity(Guid.NewGuid());
        var domainEvent = new TestDomainEvent();

        // Act
        entity.AddDomainEvent(domainEvent);

        // Assert
        entity.DomainEvents.Should().HaveCount(1);
        entity.DomainEvents[0].Should().Be(domainEvent);
    }

    [Fact]
    public void ClearDomainEvents_ShouldRemoveAllEvents()
    {
        // Arrange
        var entity = new TestEntity(Guid.NewGuid());
        entity.AddDomainEvent(new TestDomainEvent());
        entity.AddDomainEvent(new TestDomainEvent());

        // Act
        entity.ClearDomainEvents();

        // Assert
        entity.DomainEvents.Should().BeEmpty();
    }

    private sealed class TestEntity(Guid id) : Domain.Common.Entity<Guid>(id);

    private sealed class TestDomainEvent : Domain.Common.IDomainEvent;
}
