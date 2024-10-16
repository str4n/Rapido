﻿namespace Rapido.Services.Notifications.Core.Entities;

internal sealed class Template
{
    public Guid Id { get; }
    public string Name { get; private set; }
    public string Title { get; private set; }
    public string Body { get; private set; }

    public Template(Guid id, string name, string title, string body)
    {
        Id = id;
        Name = name;
        Title = title;
        Body = body;
    }

    private Template()
    {
    }
}