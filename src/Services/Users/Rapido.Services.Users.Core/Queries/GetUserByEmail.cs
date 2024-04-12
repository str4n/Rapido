using Rapido.Framework.Common.Abstractions.Queries;
using Rapido.Services.Users.Core.DTO;

namespace Rapido.Services.Users.Core.Queries;

public sealed record GetUserByEmail(string Email) : IQuery<UserDto>;