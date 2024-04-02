using Rapido.Services.Users.Core.Commands;
using Rapido.Services.Users.Core.Entities.User;

namespace Rapido.Services.Users.Core.Validators;

internal interface ISignUpValidator
{
    Task Validate(Email email, string password);
}