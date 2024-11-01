using Rapido.Framework.Common.Abstractions.Queries;

namespace Rapido.Services.Customers.Core.Common.Queries;

public sealed record CheckNameUniqueness(string Name) : IQuery<bool>;