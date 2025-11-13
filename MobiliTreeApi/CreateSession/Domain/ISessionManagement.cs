using MobiliTreeApi.Events.Models;

namespace MobiliTreeApi.CreateSession.Domain;

public interface ISessionManagement
{
    void CreateSession(Session session);
}
