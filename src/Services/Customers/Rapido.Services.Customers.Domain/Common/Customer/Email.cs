using System.Text.RegularExpressions;
using Rapido.Services.Customers.Domain.Common.Exceptions;

namespace Rapido.Services.Customers.Domain.Common.Customer;

public sealed record Email
{
    private static readonly Regex Regex =
        new(
            @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
            + "@"
            + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$");
    
    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidEmailException("Email address cannot be empty.");
        }
        
        if (!Regex.IsMatch(value))
        {
            throw new InvalidEmailException("Invalid email syntax");
        }

        Value = value;
    }
    
    public static implicit operator string(Email email) => email.Value;
    public static implicit operator Email(string email) => new(email);

    public override string ToString() => Value;
}