using Rapido.Framework.Common.Abstractions;
using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Users.Core.DTO;

namespace Rapido.Services.Users.Core.Queries;

public sealed record GetUser(Guid UserId) : IQuery<UserDto>;