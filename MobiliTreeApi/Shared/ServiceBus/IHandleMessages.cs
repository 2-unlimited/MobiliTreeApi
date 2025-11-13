using System.Threading.Tasks;

namespace MobiliTreeApi.Shared.ServiceBus;

/// <summary>
/// Fake Service Bus message handler interface.
/// In production, this would be implemented by NServiceBus or similar framework.
/// -- It's only here to facilitate the exercise, does not need to be investigated -- 
/// </summary>
public interface IHandleMessages<T> where T : class
{
    Task Handle(T message, IMessageHandlerContext context);
}

/// <summary>
/// Fake message context for Service Bus handlers.
/// </summary>
public interface IMessageHandlerContext
{
    string MessageId { get; }
    string CorrelationId { get; }
    T GetHeader<T>(string headerKey);
}

/// <summary>
/// Fake implementation of message context.
/// </summary>
public class FakeMessageHandlerContext : IMessageHandlerContext
{
    public string MessageId { get; set; }
    public string CorrelationId { get; set; }

    private readonly System.Collections.Generic.Dictionary<string, object> _headers = new();

    public T GetHeader<T>(string headerKey)
    {
        if (_headers.TryGetValue(headerKey, out var value))
        {
            return (T)value;
        }
        return default;
    }

    public void SetHeader(string key, object value)
    {
        _headers[key] = value;
    }
}
