# Design

This document aims at describing the structure of this solution. It outlines the technical concepts that are being used.

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

* **Domain objects** are objects that represent objects or concepts in the business domain, or problem domain. This includes Entities, Aggregates, and Value Objects.

* **Repositories** purpose is to retrieve and persist Aggregates (Aggregate Roots).

* **Feature** is a distinctive piece of functionality within the application. What defines the feature is the area it deals with. Structurally, it is a logical grouping of artifacts that participate in that feature, like Requests, Handlers, Controllers etc.

* **Requests** represents a request that is being made to the application. They are divided into the sub types _Commands_ or a _Queries_. There can only be one handler of a specific request.

* **Notifications** are messages that are meant to notify whomever is interest about something. They are broadcasted and can handled by one or more handlers. A specific type of notification is a _Domain event_.

* **Handlers** is the common name for a class that handles either a Request or a Notification by performing some logic. Request handlers are where the application or business logic is implemented.

These last concepts (Request, Notifications, Handlers) belong to the CQRS and Event-driven architectural patterns, as implemented by the MediatR library.

## Domain Objects

The Domain Objects are objects that represent actual concepts in your problem domain - such as business objects, like ```Invoice``` and ```Payment```.

They are the result of "Domain Modeling", and encapsulate both data and behavior as per Object-oriented programming principles.

### Entities

Entities are types of objects whose equality is determined by their identity (Id) - as indicated by an identifier, or key.

Every unique business object belongs to an entity type. For example, ``Order`` or ``Customer``. 

An entity owns its data and has behavior, or actions, defined with it, which affects it. For example, for an order the action might be completing it. Completing and order causes the order status to be set to ”completed” and preventing further updates.

If two instances of an entity type share the same identity (Id) then they represent the same object, regardless of whether the values of their properties are different. Then the challenge is about determining which individual representation is valid and most up-to-date.

In this solution, an entity is a class that derives from the abstract ```Entity<TId>``` class.

#### Aggregates

When entities are dependant on each other they form what is called an "Aggregate". 

Technically belongs here, but check the "Aggregates" section below.

In this solution, an aggregate root is a class that derivess from the abstract ```AggregateRoot<TId>``` class, which, in its turn, derives from the abstract ```Entity<TId>``` class.

### Value Objects

Value Objects are objects whose equality with other objects (of the same kind) is determined by the equality of the values of their respective properties.

Value of primitive types of programming languages (``int``, ``decimal``, ``bool``, etc) are technically value objects since they represent values. Then we have complex value types such as structs that represent sets of values.

In essence, a value objects is any domain object that does not have an identity on its own. It might be an integral part of an entity, like the steering wheel of an entity car.

In this solution, Value Objects are mostly implemented as classes deriving from the abstract ```ValueObject``` base class which provides value semantics. But they can also be structs.

#### Guarded types

Guarded types are types that encapsulate values of common types (``int``, ``decimal``, ``Guid``, ``string``, etc) to validate, or "guard" them, for invalid values. 

It also gives semantics to the value.

For example, the value of property ```Temperature``` is not just a ``decimal``. It is ``TemperatureCelsius`` that wraps the value after ensuring that it falls into the permitted range. 

And then you could involve inheritance to generalize a concept, such as temperature and allow for Farenheit as well.

Guarded types can be seen as an alternative to putting validation logic in property setters. By wrapping the logic in a type you can then also re-use it elsewhere.

A specific use case for guarded types is representing entity identifiers. Instead of having a plain ```Guid```, or whatever as an Id for an Item, you represent it as an ```ItemId``` or similar.

### Aggregates

An aggregate is a set of entities that are depending on each other. Their main characteristic is consistency.

The top or root entity within an aggregate is referred to as the "Aggregate Root".

An Aggregate Root acts as a _consistency boundary_ for all the changes to itself and the entities that it is mutually depending on. It ensures that any changes to the aggregate gets committed or saved in one consistent transaction.

An example of an aggregate is a ```Menu``` (Aggregate root) that has some ```MenuItems```. Because we are looking for consistency we can only change a particular menu item by retrieving it as part of the menu aggregate. This might be because, as a business rule, by changing some parameter for a menu item the actual serving amount could change for that menu. That is the meaning of _consistency_ within an aggregate.

Designing an aggregate is not always straightforward. You have to determine what entities is essential for the operation that the aggregate handles. Then you design it so that only the entities that are supposed to get modified can be modified.

Here are some vices:

An aggregate entity should only refer to non-essential entities by their Id. In order that it may not be directly modified from the aggregate.

## Repositories

A repository is responsible in retrieving and persisting Aggregates - more precisely Aggregate Roots. This is unlike a repository in a traditional Data Access Layer (DAL) which mainly acts as an abstraction for retrieving data from a database.

Aggregates are entities that depend on other entities for which it acts as a consistency boundary.

For the purpose of retrieving data to display it, using repositories can and should be avoided. Repositories are solely for when an aggregate is needed.

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

## Requests

TBA

## Notifications

TBA

## Services

TBA

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