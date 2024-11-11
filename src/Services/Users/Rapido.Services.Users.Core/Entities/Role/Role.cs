namespace Rapido.Services.Users.Core.Entities.Role;

public sealed record Role
{
    public string Name { get; set; }

    public static string Default => User;
    public const string User = "user";
    public const string Admin = "admin";
}