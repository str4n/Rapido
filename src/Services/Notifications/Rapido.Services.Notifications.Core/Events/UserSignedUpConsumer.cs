using MassTransit;
using Rapido.Framework.Common.Abstractions.Dispatchers;
using Rapido.Framework.Contracts.Users.Events;
using Rapido.Services.Notifications.Core.Commands;
using Rapido.Services.Notifications.Core.EF;
using Rapido.Services.Notifications.Core.Entities;

namespace Rapido.Services.Notifications.Core.Events;

internal sealed class UserSignedUpConsumer(NotificationsDbContext dbContext, IDispatcher dispatcher) : IConsumer<UserSignedUp>
{
    public async Task Consume(ConsumeContext<UserSignedUp> context)
    {
        var message = context.Message;
        var recipient = new Recipient(message.UserId, message.Email);

        await dbContext.Recipients.AddAsync(recipient);
        await dbContext.SaveChangesAsync();

        await dispatcher.DispatchAsync(new SendEmailVerification(recipient.Id, "4342342342-423424324"));
    }
}