using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MobiliTreeApi.Events;
using MobiliTreeApi.Shared.Projection;
using MobiliTreeApi.Shared.ServiceBus;

namespace MobiliTreeApi.CreateSession;

public class SessionCreatedHandler(
    ISessionProjection sessionProjection,
    ILogger<SessionCreatedHandler> logger)
    : IHandleMessages<SessionCreated>
{
    public async Task Handle(SessionCreated message, IMessageHandlerContext context)
    {
        using (logger.BeginScope(new
        {
            message.EventId,
            message.SessionId,
            context.CorrelationId,
            context.MessageId
        }))
        {
            logger.LogInformation(
                "Processing SessionCreated event for session {SessionId} at facility {FacilityId}",
                message.SessionId,
                message.ParkingFacilityId);

            if (string.IsNullOrEmpty(message.CustomerId))
            {
                logger.LogWarning("SessionCreated event has no CustomerId, skipping processing");
                throw new ArgumentNullException(nameof(message.CustomerId), "Customer Id not filled in");
            }

            if (string.IsNullOrEmpty(message.ParkingFacilityId))
            {
                logger.LogWarning("SessionCreated event has no ParkingFacilityId, skipping processing");
                throw new ArgumentNullException(nameof(message.ParkingFacilityId), "Parking Facility Id not filled in");
            }

            sessionProjection.AddSessionFromEvent(message);

            await Task.CompletedTask;
        }
    }
}
