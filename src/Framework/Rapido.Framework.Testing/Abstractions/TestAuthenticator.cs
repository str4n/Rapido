﻿using Microsoft.Extensions.Options;
using Rapido.Framework.Auth;
using Rapido.Framework.Auth.Authenticator;
using Rapido.Framework.Common.Time;

namespace Rapido.Framework.Testing.Abstractions;

public sealed class TestAuthenticator
{
    private static IAuthenticator _authenticator;
    
    public TestAuthenticator()
    {
        var options = new OptionsProvider().GetOptions<AuthOptions>("auth");
        _authenticator = new Authenticator(new UtcClock(), Options.Create(options));
    }
    
    public string GenerateJwt(Guid userId, string role, string email) =>
        _authenticator.CreateToken(userId, role, email).AccessToken;
}