﻿using Rapido.Framework.Common.Abstractions.Events;

// ReSharper disable once CheckNamespace
namespace Rapido.Messages.Events;

public sealed record FundsAdded(Guid WalletId, string TransferName, string Currency, double Amount) : IEvent;