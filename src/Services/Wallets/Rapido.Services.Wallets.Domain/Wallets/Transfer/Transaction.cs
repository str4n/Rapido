namespace Rapido.Services.Wallets.Domain.Wallets.Transfer;

public sealed record Transaction(OutgoingTransfer OutgoingTransfer, IncomingTransfer IncomingTransfer);