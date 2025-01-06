namespace Rapido.Services.Notifications.Core.Templates.Models;

public sealed record FundsAddedTemplateModel(string TransactionId, string Currency, double Amount, DateTime Date) 
    : TemplateModel;