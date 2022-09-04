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