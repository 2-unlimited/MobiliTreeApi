using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using MobiliTreeApi.Events.Models;

namespace MobiliTreeApi.Shared.ServiceBus;

/// <summary>
/// Fake in-memory message bus for simulating a Service Bus.
/// Holds messages in a queue until they are processed by handlers.
/// -- It's only here to facilitate the exercise, does not need to be investigated --
/// </summary>
public interface IFakeMessageBus
{
    void Publish<TMessage>(TMessage message) where TMessage : DomainEvent;
    IEnumerable<PendingMessage> GetPendingMessages();
    void MarkAsProcessed(Guid messageId);
}

/// <summary>
/// Represents a message waiting to be processed.
/// </summary>
public class PendingMessage
{
    public Guid MessageId { get; set; }
    public DomainEvent Event { get; set; }
    public DateTimeOffset EnqueuedTime { get; set; }
    public int DeliveryCount { get; set; }
    public string CorrelationId { get; set; }

    public PendingMessage(DomainEvent @event)
    {
        MessageId = Guid.NewGuid();
        Event = @event;
        EnqueuedTime = DateTimeOffset.UtcNow;
        DeliveryCount = 0;
        CorrelationId = @event.Context?.CorrelationId ?? Guid.NewGuid().ToString();
    }
}

/// <summary>
/// Fake message bus implementation using in-memory concurrent queue.
/// </summary>
public class FakeMessageBus : IFakeMessageBus
{
    private readonly ConcurrentQueue<PendingMessage> _messageQueue = new();
    private readonly ConcurrentDictionary<Guid, bool> _processedMessages = new();

    public void Publish<TMessage>(TMessage message) where TMessage : DomainEvent
    {
        var pendingMessage = new PendingMessage(message);
        _messageQueue.Enqueue(pendingMessage);
        
        Console.WriteLine($"[Fake Message Bus] Published: {message.GetType().Name} " +
                         $"(MessageId: {pendingMessage.MessageId}, EventId: {message.EventId})");
    }

    public IEnumerable<PendingMessage> GetPendingMessages()
    {
        // Return messages that haven't been processed yet
        return _messageQueue.Where(m => !_processedMessages.ContainsKey(m.MessageId)).ToList();
    }

    public void MarkAsProcessed(Guid messageId)
    {
        _processedMessages.TryAdd(messageId, true);
        Console.WriteLine($"[Fake Message Bus] Message {messageId} marked as processed");
    }
}
