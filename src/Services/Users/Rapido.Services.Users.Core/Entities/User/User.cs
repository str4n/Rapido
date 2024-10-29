namespace Rapido.Services.Users.Core.Entities.User;

internal sealed class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Email Email { get; set; }
    public string Password { get; set; }
    public AccountType Type { get; set; }
    public Role.Role Role { get; set; }
    public UserState State { get; set; }
    public DateTime CreatedAt { get; set; }
}