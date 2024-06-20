namespace Rapido.Services.Customers.Core.Entities.Customer;

internal enum CustomerState
{
    None,
    NotCompleted,
    Locked,
    Deleted,
    Completed,
}