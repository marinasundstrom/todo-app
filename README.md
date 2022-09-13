# Todo App

Clean Architecture and Domain-driven Design template.

To run this solution, Install .NET Tye and Docker.

## Contents

* Architecture: Clean Architecture / Layered Architecture 
* Approaches: Domain-driven design - CQRS, Event-driven
* Tests - Architecture, Application, Domain, Integration Tests
* Client app in Blazor and MudBlazor
* SignalR
* MassTransit

The app utilizes the Transactional Outbox Pattern for dispatching Domain events that have been serialized to the database.

## Screenshots

![List](/images/screenshot.png)

![Item](/images/screenshot2.png)

![Board](/images/screenshot3.png)

## Running the project

To run the app, execute this when in the solution directory:

```sh
tye run
```

Add ```--watch``` to make it recompile on changes to any project.

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



