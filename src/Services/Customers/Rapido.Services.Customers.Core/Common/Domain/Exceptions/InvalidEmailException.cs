﻿using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Customers.Core.Common.Domain.Exceptions;

internal sealed class InvalidEmailException : CustomException
{
    public InvalidEmailException(string message) : base(message, ExceptionCategory.ValidationError)
    {
    }
}