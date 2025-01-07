namespace Rapido.Services.Users.Core.User.Domain;

public sealed record Role
{
    public string Name { get; set; }

    public static string Default => User;
    public const string User = "user";
    public const string Admin = "admin";
}