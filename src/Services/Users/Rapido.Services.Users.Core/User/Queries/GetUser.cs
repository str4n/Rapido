using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Users.Core.User.DTO;

namespace Rapido.Services.Users.Core.User.Queries;

public sealed record GetUser(Guid UserId) : IQuery<UserDto>;