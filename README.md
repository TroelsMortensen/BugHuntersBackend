# Bug Hunters

## What is this?
This is a small hobby project. The idea is that people use an app on their phone to scan NFC tags placed in various places. Each NFC tag represents an insect (Bug), and the goal is to find them all. They will be hidden around campus.\
This is the backend for the mobile app.

## Tech
It is a .NET Web API, using Entity Framework Core, and for now just Sqlite. It may change later, depending on where we are going to host this, and what is possible.

## Architecture
I have aimed for some version of vertical slice architecture. Minimized the number of abstractions, and bundled almost everything for a single feature into a single folder.\
I will probably have to regularly rework the folder-structure until I'm happy.

When relevant, i.e. where there is business logic, I have attempted to apply a "functional core, imperative shell"-ish approach within a slice. This is seen with the ICoreService classes, e.g. CreateHunterService, or ChangeDisplayNameService. The Handler is then responsible for interacting with the DbContext, and the service is responsible for executing the actual logic.\
That was the idea at least. It may not have been executed all that well.

Mainly I wanted to try something different than the hexagonal shaped clean onion. I am seeing the benefits of this simpler approach.

I am also using:
* Operation Result
* REPR
* Commands and Handlers, though for now without a mediator

I am using value objects where relevant, and all entities are modelled with records, i.e. immutable data with no methods on them. This is supposedly a "better" approach, according to functional programming. We'll see. It should allow me easier testing, though.

## Testing
I saw this slightly different approach to organizing the unit tests, which I think I'll give a go. It's about structuring tests in nested classes for hopefully easier understanding of what a test case does. For example, I have one test class for the CreateHunterService class. It is structured with nested classes and methods like this:

* CreateHunterServiceTest (class)
  * CreateHunter (class)
    * Succeeds (class)
      * GivenValidHunterId (method)
      * GivenValidName (method)
      * GivenValidViaId (method)
    * Fails (class)
      * GivenInvalid... (method)
      * GivenInvalid... (method)

I have not yet concluded, whether I like the approach, but will try to stick to it for now.
