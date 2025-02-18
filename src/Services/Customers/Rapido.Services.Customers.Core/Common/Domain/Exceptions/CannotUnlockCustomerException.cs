﻿using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Customers.Core.Common.Domain.Exceptions;

internal sealed class CannotUnlockCustomerException : CustomException
{
    public CannotUnlockCustomerException() : base("Customer is unlocked.", ExceptionCategory.BadRequest)
    {
    }
}