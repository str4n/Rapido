namespace Rapido.Services.Users.Core.Entities.User;

public sealed class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Email Email { get; set; }
    public string Password { get; set; }
    public AccountType Type { get; set; }
    public Role.Role Role { get; set; }
    public bool IsActivated { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime ActivatedAt { get; set; }
    public DateTime DeletedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}