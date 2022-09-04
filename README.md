# Todo App

Clean Architecture and Domain-driven Design template.

To run this solution, Install .NET Tye and Docker.

## Contents

* Architecture: Clean Architecture / Layered Architecture 
* Approaches: Domain-driven design - CQRS, Event-driven
* Tests - Architecture, Application, Domain, Integration Tests
* Client app in Blazor
* SignalR
* MassTransit

## Screenshots

![Screenshot](/images/screenshot.png)

## Running the project

To run the app, execute this when in the solution directory:

```sh
tye run
```

Add ```--watch``` to make it recompile on changes to any project.

## Adapting this for your own project

To adapt this into your own project:

* In, ```Application```, remove the Todos folder, containing Commands and Queries. Add your own.
* In ```Domain```, replace Entities, Enums and Events etc with your domain.
* In ```Infrastructure```, update ApplicationDbContext and define your own Repositories.
* In ```Presentation```, empty Controllers and Hubs. Create your own.

And for the client-side, just empty ```ClientApp``` and build your own. Or, replace with whatever you like.

The ```Client``` generates client-code from Open API/Swagger file. Update that as you develop.

## Testing

This solution contains the following test projects:

* ```Architecture.Tests``` - verifies that the integritity of the architecture, the dependencies between projects, have not been broken.

* ```Specs``` verifies Use Cases (scenarios). They are being grouped by Feature - not corresponding to User Stories. Implementation TBD.

* ```IntegrationTests``` verifies that the Web API works as intended.

* ```Application.Tests``` - verifies the application logic - commands, queries, event handlers, and services.

* ```Domain.Tests``` - verifies the domain logic, i.e. domain objects and services.

* ```Infrastructure.Tests``` - verifies the infrastructure layer works as intended.

* ```ClientApp.Tests``` - verifies that the UI components behave the way that they should.



