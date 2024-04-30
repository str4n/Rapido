using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Services.Wallets.Application.Owners.Commands;

public sealed record TransformOwnerIntoCorporate(Guid OwnerId, string TaxId) : ICommand;