﻿using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Customers.Core.Common.Exceptions;

internal sealed class InvalidCustomerTypeException : CustomException
{
    public InvalidCustomerTypeException() : base("Customer type is invalid.", ExceptionCategory.ValidationError)
    {
    }
}