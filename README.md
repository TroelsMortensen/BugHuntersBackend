# Bug Hunters

Note: I am regularly updating this README, as I go along. I have changed plans and ideas, but may leave the description in, so this is perhaps more a document of the process, rather than documentation of the product. We'll see.\

## What is this?
This is a small hobby project. The idea is that people use an app on their phone to scan NFC tags placed in various places. Each NFC tag represents an insect (Bug), and the goal is to find them all. They will be hidden around campus.\
This is the backend for the mobile app.

## Tech
It is a .NET Web API, using Entity Framework Core, and for now just Sqlite. It may change later, depending on where we are going to host this, and what is possible.

## Architecture
I have aimed for some version of vertical slice architecture. Minimized the number of abstractions, and bundled almost everything for a single feature into a single folder.\
I will probably have to regularly rework the folder-structure until I'm happy.

### Functional Core/Imperative Shell
I started out with this idea, without really knowing what it was. I imagined some kind of hexagonal approach, but with a hopefully slightly simpler structure.\
When relevant, i.e. where there is business logic, I attempted to apply a "functional core, imperative shell"-ish approach within a slice. This is previously seen with the ICoreService classes, e.g. CreateHunterService, or ChangeDisplayNameService. 
The Handler is then responsible for interacting with the DbContext, and the service is responsible for executing the actual logic.\
That was the idea at least. It may not have been executed all that well. The point of this is to make the core business logic easier to unit test.

Mainly I wanted to try something different than the hexagonal shaped clean onion. I am seeing the benefits of this simpler approach.

### Railway oriented programming
I have since removed the above FC/IS idea. It was just more classes than I cared about, it seemed over-engineered. So, I've gone even more functional, stripped things away. I am now heavily inspired by the Railway Oriented Programming approach, and using my Result type to handle this.\
The application can be seen in the EndPoints, each endpoint is a "railway", which will do whatever needs to be done to get from input request to output response.\
Most of the code relevant to a feature will be in just that Endpoint class. Primarily this is a few functions. Some functions used will be on the DbContext as extension methods. And there will be a few functions on various entity modules (i.e. defined with the entity in a separate static class).\
I am then hopefully diligent about refactoring whenever I see a pattern, so that I can extract it to a more general function, and use that instead. For example the BugHunterContext::HunterExists function, which is used in multiple endpoints. This function does look a bit funny, but that's what I needed at the time.

The Railway Oriented Programming approach has also resulted in quite an elaborate Result class and module (associated functions). It has exploded in overloaded functions because of the need for asynchronous variations. I am not sure if this is the best way to do it, but it seems to work for now. I do enjoy this approach, and find it interesting to work with.\

### Notable atterns
I am using:
* Operation Result
* REPR pattern
* Value Objects
* Aggregates (maybe..?)
* Strongly typed IDs

I am using Value Objects where relevant, and all entities are modelled with records, i.e. immutable data with no methods on them. This is supposedly a "better" approach, according to functional programming. 
We'll see. It should allow me easier testing, though.\
On the other hand it probably means moving away from DDD aggregates, which I have come to appreciate.

## Testing

I have planned different testing approaches:
* Integration testing for every endpoint (at least that's the plan eventually), which includes the Web API and a SQLite database.

I'm probably not going to be doing much unit testing, unless strictly necessary. This is because of the idea of testing against interfaces rather than implementations. I don't want to tie my tests to the implementation details of the web app. And the outer-most interface is the endpoint.\
For example, I have done a major rework of the internal architecture, without most of the integration tests being affected. I do appreciate this.

### Unit testing
This section is then probably deprecated. I'm leaving it in, because this was my initial thought, and I may return to it. Or it may have value for the future.

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

I have not yet concluded, whether I like the approach, but will try to stick to it for now.\
Update: it makes it less clear what each method is doing, if only looking at that method, because the full "test name" is spread across the nested classes. Maybe I will revisit this approach later.

### Integration testing
I'm starting to conclude that I will primarily do integration testing. I have a few unit tests, but I think I will focus on integration tests.

It turns out they are actually not that slow, for now at least. And it means I don't tie to the implementation details of the web app. I can change the implementation, and as long as the API is the same, the tests should still pass.\
So, for all integration tests, I will use only the Web API, and not for example the database directly. Any kind of data, which needs to be setup before the test, will be done through the API.