# Bug Hunters
This is a small hobby project. The idea is that people use an app on their phone to scan NFC tags placed in various places. Each NFC tag represents an insect (Bug), and the goal is to find them all.\
This is the backend for the mobile app.

It is a .NET Web API, using Entity Framework Core, and for now just Sqlite. It may change later, depending on where we are going to host this, and what is possible.

I have aimed for some version of vertical slice architecture. And when relevant, I have attempted to apply a "functional core, imperative shell"-ish approach within a slice. That was the idea at least. It may not have been executed all that well.\
Mainly I wanted to try something different than the hexagonal shaped clean onion.
