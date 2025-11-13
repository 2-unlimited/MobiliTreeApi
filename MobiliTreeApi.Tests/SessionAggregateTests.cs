using System;
using System.Linq;
using System.Threading.Tasks;
using MobiliTreeApi.Shared.Domain;
using Microsoft.Extensions.Logging.Abstractions;
using MobiliTreeApi.Events;
using MobiliTreeApi.Events.Models;
using Xunit;
using MobiliTreeApi.Shared.Storage;

namespace MobiliTreeApi.Tests;

public class SessionAggregateTests
{
    [Fact]
    public void CreateSession_AppendsSessionCreatedEvent()
    {
        // Arrange
        var aggregate = new SessionAggregate();
        var session = new Session
        {
            ParkingFacilityId = "pf001",
            CustomerId = "c001",
            StartDateTime = DateTime.Parse("2025-01-15 10:00"),
            EndDateTime = DateTime.MinValue
        };
        var eventContext = new EventContext(
            DateTimeOffset.UtcNow,
            UserId: "c001",
            SourceSystem: "TestSystem"
        );

        // Act
        var result = aggregate.CreateSession(session, eventContext);

        // Assert
        Assert.True(result.UnitOfWorkShouldComplete);
        var uncommittedEvents = aggregate.GetUncommittedEvents();
        Assert.Single(uncommittedEvents);
        Assert.IsType<SessionCreated>(uncommittedEvents.First());

        var sessionCreatedEvent = (SessionCreated)uncommittedEvents.First();
        Assert.Equal("pf001-c001-20250115100000", sessionCreatedEvent.SessionId);
        Assert.Equal("pf001", sessionCreatedEvent.ParkingFacilityId);
        Assert.Equal("c001", sessionCreatedEvent.CustomerId);
    }

    [Fact]
    public void CreateSession_Idempotent_ReturnsWasAlreadyDone()
    {
        // Arrange
        var aggregate = new SessionAggregate();
        var session = new Session
        {
            ParkingFacilityId = "pf001",
            CustomerId = "c001",
            StartDateTime = DateTime.Parse("2025-01-15 10:00"),
            EndDateTime = DateTime.UtcNow
        };
        var eventContext = new EventContext(DateTimeOffset.UtcNow, "c001", "TestSystem");

        // Act - Create session first time
        var firstResult = aggregate.CreateSession(session, eventContext);

        // Act - Try to create same session again (idempotency check)
        var secondResult = aggregate.CreateSession(session, eventContext);

        // Assert
        Assert.True(firstResult.UnitOfWorkShouldComplete);
        Assert.False(secondResult.UnitOfWorkShouldComplete);

        // Only one event should be appended
        var uncommittedEvents = aggregate.GetUncommittedEvents();
        Assert.Single(uncommittedEvents);
    }

    [Fact]
    public async Task SessionAggregateRepository_SavesAndLoadsEvents()
    {
        // Arrange
        var fakeMessageBus = new MobiliTreeApi.Shared.ServiceBus.FakeMessageBus();
        var logger = NullLogger<FakeSessionAggregateRepository>.Instance;
        var repository = new FakeSessionAggregateRepository(fakeMessageBus, logger);
        var sessionId = "pf001-c001-20250115100000";

        // Act - Create and save aggregate
        var aggregate1 = await repository.LoadOrCreateSessionAggregateAsync(sessionId);
        var session = new Session
        {
            ParkingFacilityId = "pf001",
            CustomerId = "c001",
            StartDateTime = DateTime.Parse("2025-01-15 10:00"),
            EndDateTime = DateTime.UtcNow
        };
        var eventContext = new EventContext(DateTimeOffset.UtcNow, "c001", "TestSystem");

        aggregate1.CreateSession(session, eventContext);
        await repository.SaveChangesAsync();

        // Act - Load same aggregate again (should replay events)
        var logger2 = NullLogger<FakeSessionAggregateRepository>.Instance;
        var repository2 = new FakeSessionAggregateRepository(fakeMessageBus, logger2);
        var aggregate2 = await repository2.LoadOrCreateSessionAggregateAsync(sessionId);

        // Assert - Second aggregate should have state from first aggregate's events
        var result = aggregate2.CreateSession(session, eventContext);
        Assert.False(result.UnitOfWorkShouldComplete); // Session already created
    }
}
