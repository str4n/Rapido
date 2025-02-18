﻿namespace Rapido.Services.Users.Core.UserActivation.Domain;

public sealed class UserActivationToken
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresOn { get; set; }
    public DateTime CreatedAt { get; set; }
}