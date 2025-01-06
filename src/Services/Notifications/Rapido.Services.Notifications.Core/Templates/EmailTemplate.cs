namespace Rapido.Services.Notifications.Core.Templates;

public sealed record EmailTemplate(string Subject, string Body)
{
    public const string ActivationEmailTemplateName = "ActivateUser";
    public const string FundsAddedTemplateName = "FundsAdded";
    public const string FundsDeductedTemplateName = "FundsDeducted";
}