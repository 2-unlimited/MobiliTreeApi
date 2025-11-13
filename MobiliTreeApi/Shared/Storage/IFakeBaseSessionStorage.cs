using System.Collections.Generic;
using MobiliTreeApi.Events.Models;

namespace MobiliTreeApi.Shared.Storage;

/// <summary>
/// Fake implementation that emulates a storage through a SDK.
/// This is a low-level storage mechanism with only basic CRUD operations.
/// -- It's only here to facilitate the exercise, does not need to be investigated --
/// </summary>
public interface IFakeBaseSessionStorage
{
    void Upsert(string id, Session document);
    Session Read(string id);
    List<Session> Query(System.Func<Session, bool> predicate);
    List<Session> GetAll();
}