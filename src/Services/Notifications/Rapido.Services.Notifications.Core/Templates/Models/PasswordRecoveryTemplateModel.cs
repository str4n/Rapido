namespace Rapido.Services.Notifications.Core.Templates.Models;

public sealed record PasswordRecoveryTemplateModel(string RecoveryToken) : TemplateModel;