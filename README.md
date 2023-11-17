# ASP.NET Core 8.0 - Minimal API Example.

ASP.NET Core 8.0 - Minimal API Example - Todo API implementation using ASP.NET Core Minimal API, Entity Framework Core, Token authentication, Versioning, Unit Testing, Integration Testing and Open API.

[![Build and Deployment](https://github.com/anuraj/MinimalApi/actions/workflows/main.yml/badge.svg)](https://github.com/anuraj/MinimalApi/actions/workflows/main.yml)

## Features
### November 17, 2023
* Upgraded to .NET 8

### September 22, 2023
* Upgraded to .NET 8 RC1 - [More details](https://devblogs.microsoft.com/dotnet/announcing-dotnet-8-rc1/?WT.mc_id=DT-MVP-5002040)
* No other .NET 8 features implemented.

### November 29, 2022
* Implemented Rate Limiting support for Web API in .NET 7 - [Learn more about this feature](https://learn.microsoft.com/aspnet/core/performance/rate-limit?view=aspnetcore-7.0&WT.mc_id=DT-MVP-5002040)
* Removed GraphQL support.

### November 22, 2022
* Publishing Code coverage results as artifact

### November 21, 2022
* DotNet CLI - Container image publish support added - [Learn more about this feature](https://devblogs.microsoft.com/dotnet/announcing-builtin-container-support-for-the-dotnet-sdk/?WT.mc_id=DT-MVP-5002040)
* Modified authentication code to support `dotnet user-jwts`. Removed the `token` endpoint
* How to create token using `dotnet user-jwts`.
	* If the dotnet tool not exist, you may need to install it first.
	* Execute the command - `dotnet user-jwts create --claim Username=user1 --claim Email=user1@example.com --name user1`. 
	* This will generate a token and you can use this token in the Swagger / Open API.

You can find more details here - [Manage JSON Web Tokens in development with dotnet user-jwts](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/jwt-authn?view=aspnetcore-7.0&tabs=windows&WT.mc_id=DT-MVP-5002040)

### November 18, 2022
* Moved from .NET 6.0 to .NET 7.0
* Endpoint Filters added - [Learn more about this feature](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis/min-api-filters?view=aspnetcore-7.0&WT.mc_id=DT-MVP-5002040)

### October 18, 2022
* NuGet packages upgraded to RC.

### July 25, 2022
* Route groups Implemented - [Learn more about this feature](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-7.0&WT.mc_id=DT-MVP-5002040#route-groups)
* Code coverage implemented.

### July 24, 2022
* Upgraded to .NET 7 - Preview.
* Added Unit Tests (Unit Tests support for Minimal API available in .NET 7)

### July 22, 2022
* Implemented Paging.

### July 21, 2022
* Validation support using FluentValidation
* Refactoring and fixed all the warnings.
* Graph QL Authentication

### December 1, 2021
* Implemented DTO for Input and Output.
* Bug Fix - the /history endpoint was not returning any data.

### November 24, 2021
* Token Authentication and Open API changes related to that.

### November 14, 2021
* GraphQL Implementation using HotChocolate
	- Query
	- Mutation
	- Subscription
	
### November 11, 2021
* CRUD operations using Minimal API .NET 6.0 and Sql Server
* Health Checks implementation for Minimal APIs
* Open API - Support for Tags
* EF Core new features 
	- Temporal Tables in Sql Server
	- Run migration using EF Bundles
