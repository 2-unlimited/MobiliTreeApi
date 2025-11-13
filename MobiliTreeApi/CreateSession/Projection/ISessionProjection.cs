using MobiliTreeApi.Events;

namespace MobiliTreeApi.Shared.Projection;

public partial interface ISessionProjection
{
    void AddSessionFromEvent(SessionCreated sessionCreatedEvent);
}