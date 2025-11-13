using MobiliTreeApi.Shared.Storage;

namespace MobiliTreeApi.Shared.Projection;

public partial class SessionProjection(ISessionStorage storage) : ISessionProjection;
