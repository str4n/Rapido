﻿using Rapido.Framework.Common.Abstractions.Events;

namespace Rapido.Framework.Contracts.Customers.Events;

public sealed record CustomerUnlocked(Guid CustomerId) : IEvent;