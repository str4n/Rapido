﻿using Rapido.Framework.Common.Abstractions.Events;

// ReSharper disable once CheckNamespace
namespace Rapido.Messages;

public sealed record CustomerAddressChanged(Guid CustomerId) : IEvent;