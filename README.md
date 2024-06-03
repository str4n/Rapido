# Rapido

Rapido is the virtual funds transfer app built using **Microservices** architecture, written in [.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).

Each service is an independent web application with its custom architecture, and the overall integration between the services is mostly based on the **event-driven** approach with **shared contracts**.

Depending on the service complexity, different architectural styles are being used, including simple **CRUD** approach, along with **CQRS**, **Clean Architecture** and **Domain-Driven Design**.

The database is being used is [PostgreSQL](https://www.postgresql.org/) & [EntityFrameworkCore](https://learn.microsoft.com/en-us/ef/core/) as ORM, moreover application uses [Redis](https://redis.io/) as a cache.
<hr>




<hr>

# Solution structure

## API Gateway
An **API Gateway** acts as a single entry point for clients to access microservices in a system. It routes requests to the appropriate **microservices** using **[YARP](https://github.com/microsoft/reverse-proxy)** as revese proxy.

## Services
Autonomous applications with the different set of responsibilities, decoupled from each other - there's reference between the services and shared packages, and the synchronous communication & asynchronous integration (via events) is based on shared contracts approach.


**Customers** - managing the customers (create, verify, lock).

**Users** - managing the users/identity (register, login, permissions etc.).

**Wallets** - managing the wallets and owners, responsible for fund transfers(transfer, add, deduct (funds))

**Payments** - managing the deposits and withdrawals from account(integrated with stripe).

**Notifications** - sending notifications via email (transfer confirmation, account verification, password recovery etc).

## Saga
Sample Saga pattern implementation for transactional handling the business processes spanning across the distinct modules.

## Framework
The set of shared components for the common abstractions & cross-cutting concerns. 
In order to achieve even better decoupling and decentralization, it's split into the separate projects.
