﻿using Rapido.Framework.Common.Exceptions;

namespace Rapido.Services.Users.Core.Exceptions;

internal sealed class UserAlreadyExistsException : CustomException
{
    public UserAlreadyExistsException(string message) : base(message, ExceptionCategory.AlreadyExists)
    {
    }
}