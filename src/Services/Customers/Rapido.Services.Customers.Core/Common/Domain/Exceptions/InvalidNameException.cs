﻿using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Customers.Core.Common.Domain.Exceptions;

internal sealed class InvalidNameException : CustomException
{
    public InvalidNameException(string message) : base(message, ExceptionCategory.ValidationError)
    {
    }
}