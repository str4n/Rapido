using MassTransit;
using Rapido.Framework.Contracts.Customers.Commands;
using Rapido.Framework.Contracts.Customers.Events;
using Rapido.Framework.Contracts.Notifications;
using Rapido.Framework.Contracts.Notifications.Commands;
using Rapido.Framework.Contracts.Notifications.Events;
using Rapido.Framework.Contracts.Users.Events;
using Rapido.Framework.Contracts.Wallets.Commands;
using Rapido.Framework.Contracts.Wallets.Events;

namespace Rapido.Saga.Sagas;

public class AccountSetUpSagaData : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public Guid AccountId { get; set; }
    public string Email { get; set; }
    public string AccountType { get; set; }
    public bool UserActivated { get; set; }
    public bool CustomerCreated { get; set; }
    public bool CustomerCompleted { get; set; }
    public bool OwnerCreated { get; set; }
    public bool WalletCreated { get; set; }
    public bool AccountSetUpCompleted { get; set; }
}

public class AccountSetUpSaga : MassTransitStateMachine<AccountSetUpSagaData>
{
    public State Activation { get; set; }
    public State CustomerCreation { get; set; }
    public State CustomerCompleting { get; set; }
    public State OwnerCreation { get; set; }
    public State WalletCreation { get; set; }
    public State AccountCompletion { get; set; }
    
    public Event<UserSignedUp> UserSignedUp { get; set; }
    public Event<UserActivated> UserActivated { get; set; }
    public Event<CustomerCreated> CustomerCreated { get; set; }
    public Event<IndividualCustomerCompleted> IndividualCustomerCompleted { get; set; }
    public Event<CorporateCustomerCompleted> CorporateCustomerCompleted { get; set; }
    public Event<OwnerCreated> OwnerCreated { get; set; }
    public Event<WalletCreated> WalletCreated { get; set; }

    public AccountSetUpSaga()
    {
        InstanceState(x => x.CurrentState);

        Event(() => UserSignedUp, e => e.CorrelateById(m => m.Message.UserId));
        Event(() => UserActivated, e => e.CorrelateById(m => m.Message.UserId));
        Event(() => CustomerCreated, e => e.CorrelateById(m => m.Message.CustomerId));
        Event(() => IndividualCustomerCompleted, e => e.CorrelateById(m => m.Message.CustomerId));
        Event(() => CorporateCustomerCompleted, e => e.CorrelateById(m => m.Message.CustomerId));
        Event(() => OwnerCreated, e => e.CorrelateById(m => m.Message.OwnerId));
        Event(() => WalletCreated, e => e.CorrelateById(m => m.Message.OwnerId));
        
        Initially(
            When(UserSignedUp)
            .Then(context =>
            {
                context.Saga.AccountId = context.Message.UserId;
                context.Saga.Email = context.Message.Email;
                context.Saga.AccountType = context.Message.AccountType;
            })
            .TransitionTo(Activation)
            .Publish(context => new SendActivationEmail(context.Message.UserId, "test-value")));
        
        During(Activation, 
            When(UserActivated)
                .Then(context => context.Saga.UserActivated = true)
                .TransitionTo(CustomerCreation)
                .Publish(context => new CreateCustomer(context.Message.UserId, context.Saga.Email, context.Saga.AccountType)));
        
        During(CustomerCreation, 
            When(CustomerCreated)
                .Then(context => context.Saga.CustomerCreated = true)
                .TransitionTo(CustomerCompleting));
        
        During(CustomerCompleting, 
            When(IndividualCustomerCompleted)
                .Then(context => context.Saga.CustomerCompleted = true)
                .TransitionTo(OwnerCreation)
                .Publish(context => new CreateIndividualOwner(
                    context.Message.CustomerId, 
                    context.Message.Name, 
                    context.Message.FullName, 
                    context.Message.Nationality)), 
            When(CorporateCustomerCompleted)
                .Then(context => context.Saga.CustomerCompleted = true)
                .TransitionTo(OwnerCreation)
                .Publish(context => new CreateCorporateOwner(
                    context.Message.CustomerId, 
                    context.Message.Name, 
                    context.Message.TaxId, 
                    context.Message.Nationality)));
        
        During(OwnerCreation, 
            When(OwnerCreated)
                .Then(context => context.Saga.OwnerCreated = true)
                .TransitionTo(WalletCreation)
                .Publish(context => new CreateWallet(context.Message.OwnerId, context.Message.Nationality)));
        
        During(WalletCreation, 
            When(WalletCreated)
                .Then(context =>
                {
                    context.Saga.WalletCreated = true;
                    context.Saga.AccountSetUpCompleted = true;
                })
                .TransitionTo(AccountCompletion)
                .Finalize());
    }
}