using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Services.Customers.Core.Common.Commands;

public sealed record ChangeNationality(Guid Id, string Nationality) : ICommand;