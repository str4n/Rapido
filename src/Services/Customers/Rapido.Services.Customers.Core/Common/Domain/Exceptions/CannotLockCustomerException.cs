﻿using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Customers.Core.Common.Domain.Exceptions;

internal sealed class CannotLockCustomerException : CustomException
{
    public CannotLockCustomerException() : base("Cannot lock customer.", ExceptionCategory.BadRequest)
    {
    }
}