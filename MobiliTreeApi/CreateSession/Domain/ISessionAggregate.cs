using MobiliTreeApi.Events.Models;
using MobiliTreeApi.Shared.Results;

namespace MobiliTreeApi.Shared.Domain;

public partial interface ISessionAggregate
{
    ICommandResult CreateSession(Session session, EventContext eventContext);
}