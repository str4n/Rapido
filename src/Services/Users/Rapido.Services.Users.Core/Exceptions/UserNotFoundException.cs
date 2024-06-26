﻿using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Users.Core.Exceptions;

internal sealed class UserNotFoundException : CustomException
{
    public UserNotFoundException(string message) : base(message, ExceptionCategory.NotFound)
    {
    }
}