namespace Rapido.Services.Notifications.Core.Entities;

internal sealed class Template
{
    public Guid Id { get; }
    public string Name { get; private set; }
    public string Subject { get; private set; }
    public string TemplatePath { get; private set; }

    public Template(Guid id, string name, string subject, string templatePath)
    {
        Id = id;
        Name = name;
        Subject = subject;
        TemplatePath = templatePath;
    }

    private Template()
    {
    }
}