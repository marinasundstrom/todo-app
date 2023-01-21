# Design

## Vertical Slice Architecture

Logically sliced vertically - by feature

Eliminate unnecessary layers
Emphasis on logical separation
Maximize Separation of Concern
Reduce complexity and cognitive load
Less projects

Advantages of Clean Architecture (CA)

## Domain-driven design (DDD)

TBA

## Layers

The appliction consists of layers which are represented by 3 projects.

The layers are:

* Application
* Infrastructure
* Web

### Dependencies between layers

* **Application** has no dependencies on the other layers.

* **Infrastructure** has a dependency on Application. In that way it knows about the necessary interfaces to implement for services, data access, and repositories.

* **Web** is the hosting application, and it knows about the other two layers.

## Concepts

Here are some concepts in this application that are worth knowing about:

* **Feature** is a distinctive piece of functionality within the application. Structurally, it is a logical grouping of artifacts that participate in that feature, like Requests, Handlers, Controllers etc.

* **Requests** represents a request that is being made to the application. They are divided into the sub types _Commands_ or a _Queries_.

* **Notifications** are messages that are broadcasted. A specific type of notification is a _Domain event_.

* **Handlers** is the common name for a class that handles either a Request or a Notification by performing some logic. Request handlers are where the application or business logic is implemented.

These last concepts (Request, Notifications, Handlers) belong to the CQRS and Event-driven architectural patterns, as implemented by the MediatR library.

## Features

Types are grouped by feature, in the Features folder.

In a feature folder you might find these items:

* Request (Commands and Queries)
* Notifications (Events)
* Handlers
* Controllers
* SignalR Hubs
* Services
* Other classes pertaining to the current feature.

## Results

The ``Result`` class(es) are used for returning results, or eventual errors in the form of ``Error`` objects.

Although results are commonly used in handlers, they can be used in services and domain objects.

### Errors

Errors are known conditions that are represented by ``Error`` objects. Errors should be handled within the application logic.

Ultimately, errors should preferably be surfaced to the user via the API.

## Exceptions

Exceptions are exceptional conditions that normally causes the application to unexpectedly crash.

They are usually caused by unexpected input or by the system itself, for instance, IO or Network exceptions. They can in some cases be prevented, or at least you can make sure to catch and handle them.

A know exception should be handled and projected as a ``Result`` containing an well-defined ``Error`` object which describes the error within the context it occured.