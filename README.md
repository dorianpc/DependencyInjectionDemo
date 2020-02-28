# DependencyInjectionDemo
Dependency Injection with Autofac

Frameworks/Libraries
====================
* Net Core 3.0
* Autofac
* Dapper
* NUnit
* Moq
* Sqlite


Sample console app that injects data access concrete types for SQL Server or Sqlite.

Download Sqlite, create the DemoDB.db file and create a Person table with Id, FirstName, LastName to use with project. 
Same table is required with Sql Server.

For testing, I composed the vscode tasks.json file with tasks to delete and copy any of the 2 data access dlls into the client directory, which are dynamically loaded with reflection and registered with the DI container.

This solution is for educational purposes. 

