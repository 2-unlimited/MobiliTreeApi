using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MobiliTreeApi.Events.Models;

namespace MobiliTreeApi.Shared.ServiceBus;

/// <summary>
/// Background service that polls the fake message bus and processes messages.
/// Simulates a Service Bus message processing in a real system.
/// -- It's only here to facilitate the exercise, does not need to be investigated --
/// </summary>
public class FakeMessageBusProcessor(
    IFakeMessageBus messageBus,
    IServiceProvider serviceProvider,
    ILogger<FakeMessageBusProcessor> logger)
    : BackgroundService
{
    private readonly TimeSpan _pollingInterval = TimeSpan.FromSeconds(1);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Fake Message Bus Processor started. Polling every {Interval} seconds",
            _pollingInterval.TotalSeconds);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessPendingMessages(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing messages in Fake Message Bus Processor");
            }

            await Task.Delay(_pollingInterval, stoppingToken);
        }

        logger.LogInformation("Fake Message Bus Processor stopped");
    }

    private async Task ProcessPendingMessages(CancellationToken stoppingToken)
    {
        var pendingMessages = messageBus.GetPendingMessages().ToList();

        if (pendingMessages.Count == 0)
        {
            return;
        }

        logger.LogInformation("Processing {Count} pending message(s)", pendingMessages.Count);

        foreach (var pendingMessage in pendingMessages.TakeWhile(_ => !stoppingToken.IsCancellationRequested))
        {
            try
            {
                await ProcessMessage(pendingMessage, stoppingToken);
                messageBus.MarkAsProcessed(pendingMessage.MessageId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Failed to process message {MessageId} of type {EventType}. DeliveryCount: {DeliveryCount}",
                    pendingMessage.MessageId,
                    pendingMessage.Event.GetType().Name,
                    pendingMessage.DeliveryCount);

                pendingMessage.DeliveryCount++;

                if (pendingMessage.DeliveryCount >= 3)
                {
                    logger.LogWarning("Message {MessageId} exceeded max delivery attempts, marking as processed",
                        pendingMessage.MessageId);
                    messageBus.MarkAsProcessed(pendingMessage.MessageId);
                }
            }
        }
    }

    private async Task ProcessMessage(PendingMessage pendingMessage, CancellationToken stoppingToken)
    {
        var eventType = pendingMessage.Event.GetType();

        logger.LogInformation("Processing message {MessageId} of type {EventType}",
            pendingMessage.MessageId,
            eventType.Name);

        using var scope = serviceProvider.CreateScope();

        var context = new FakeMessageHandlerContext
        {
            MessageId = pendingMessage.MessageId.ToString(),
            CorrelationId = pendingMessage.CorrelationId
        };

        var handled = await TryHandleMessage(scope.ServiceProvider, pendingMessage.Event, context);

        if (!handled)
        {
            logger.LogWarning("No handler found for message type {EventType}", eventType.Name);
        }
    }

    private static async Task<bool> TryHandleMessage(IServiceProvider scopedProvider, DomainEvent domainEvent, IMessageHandlerContext context)
    {
        var eventType = domainEvent.GetType();
        var handlerInterfaceType = typeof(IHandleMessages<>).MakeGenericType(eventType);

        var handler = scopedProvider.GetService(handlerInterfaceType);

        if (handler == null)
        {
            return false;
        }

        // Invoke the Handle method using reflection
        var handleMethod = handlerInterfaceType.GetMethod("Handle");
        if (handleMethod == null)
            return false;
        var task = (Task)handleMethod.Invoke(handler, [domainEvent, context]);
        if (task != null)
            await task;
        return true;
    }
}
