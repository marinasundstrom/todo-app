# Todo App

Clean Architecture and Domain-driven Design template.

To run this solution, Install .NET Tye and Docker.

Watch video [here](https://youtu.be/hlk3sKIVXgM), and an older video also showing health checks and telemetry [here](https://youtu.be/UW-iDps48BI).

This template is the result of many years of research and trying. Some of the influences  are Jason Taylor's [Clean Architecture template](https://github.com/jasontaylordev/CleanArchitecture), and topics brought up by [CodeOpinion](https://www.youtube.com/channel/UC3RKA4vunFAfrfxiJhPEplw) and [Milan Jovanovic](https://www.youtube.com/c/MilanJovanovicTech). Thank you all!

If you are interested, check out my earlier projects: [YourBrand](https://github.com/marinasundstrom/YourBrand).

## Why?

Having a stable ground on which to build a project helps you to focus on what really matters: Features and Use Cases.

Following the patterns let you worry less about technical concerns.

In this particular project, you get the project structure and services you need for dispatching domain events for free. It also helps you test your code.

Feel free to adopt and modify this to your own needs. Instructions below.

## Contents

* Architecture: Clean Architecture / Layered Architecture 
* Approaches: Domain-driven design - CQRS, Event-driven
* Tests - Architecture, Application, Domain, Integration Tests
* Client app in Blazor and MudBlazor
  * Localization
  * Theming - Dark mode
* SignalR: Real-time server-browser communication via WebSocket
* MassTransit - RabbitMQ as transport
* OpenTelemetry - Zipkin for viewing traces
* Health checks
* Identity Server - for identity management and auth

The app utilizes the Transactional Outbox Pattern for dispatching Domain events that have been serialized to the database.

## Screenshots

![List](/images/screenshot.png)

![Item](/images/screenshot2.png)

![Board](/images/screenshot3.png)

![Board (Dark mode)](/images/screenshot4.png)

## Running the project

To run the app, execute this when in the solution directory:

```sh
tye run
```

Add ```--watch``` to make it recompile on changes to any project.

### Seeding the databases
Web

```sh
dotnet run -- --seed
```

IdentityService

```sh
dotnet run -- /seed
```

### Login credentials

```
Username: alice 
Password: alice

Username: bob 
Password: bob
```

## Adapting this for your own project

In order to adapt this into your own project, you should do this:

1. Replace the name of the root namespace ”TodoApp” with your desired name.
2. Rename the solution file.
3. Do case sensitive replacement of ”Todo” and ”todo”. This will rename variables and types. Change filenames accordingly. 

Proceed to perform changes to the structure of each project:

* In ```Application```, remove the Todos folder, containing Commands and Queries. Add your own.
* In ```Domain```, replace Entities, Enums and Events etc with your domain.
* In ```Infrastructure```, update ApplicationDbContext and define your own Repositories.
* In ```Presentation```, empty Controllers and Hubs. Create your own.

And for the client-side, just empty ```ClientApp``` and build your own. Or, replace with whatever tech you like, e.g. an app build with a JS framework, like React.

The ```Client``` project is set up to generate C# client code from Open API/Swagger file. Update that file as you develop.

## Testing

This solution contains the following test projects:

* ```Architecture.Tests``` - verifies that the integritity of the architecture, the dependencies between projects, have not been broken.

* ```Specs``` verifies Use Cases (scenarios). They are being grouped by Feature - not corresponding to User Stories. Implementation TBD.

* ```IntegrationTests``` verifies that the integration points - Web API, SignalR, MassTransit - work as intended.

* ```Application.Tests``` - verifies the application logic - commands, queries, event handlers, and services.

* ```Domain.Tests``` - verifies the domain logic, i.e. domain objects and services.

* ```Infrastructure.Tests``` - verifies that the infrastructure layer works as intended.

* ```ClientApp.Tests``` - verifies that the UI components behave the way that they should.



