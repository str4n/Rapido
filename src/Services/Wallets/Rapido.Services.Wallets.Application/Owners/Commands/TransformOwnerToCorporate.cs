﻿using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Services.Wallets.Application.Owners.Commands;

public sealed record TransformOwnerToCorporate(Guid OwnerId, string TaxId) : ICommand;