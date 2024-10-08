﻿using Rapido.Framework.Common.Abstractions.Commands;

namespace Rapido.Services.Customers.Application.Common.Commands;

public sealed record ChangeNationality(Guid Id, string Nationality) : ICommand;