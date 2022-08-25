# ASP.NET Core 6.0 - Minimal API Example.

ASP.NET Core 6.0 - Minimal API Example - Todo API implementation using ASP.NET Core Minimal API, GraphQL, Entity Framework Core, Token authentication and Open API.

[![Build and Deployment](https://github.com/anuraj/MinimalApi/actions/workflows/main_minimalapi-demo.yml/badge.svg)](https://github.com/anuraj/MinimalApi/actions/workflows/main_minimalapi-demo.yml)

## Features

### August 25, 2022
* Using the C# 10 feature - `Global Using Directive`
* Implemented Custom Binding for File Upload - Since `[FromForm]` attribute not available.

### August 11, 2022
* Implemented Web API versioning - with Open API support.

### July 24, 2022
* Upgraded to ASP.NET Core 7.0 - Checkout the branch [dev/aspnet7.0](https://github.com/anuraj/MinimalApi/tree/dev/aspnet7.0)

### July 22, 2022
* Implemented Paging.

### July 21, 2022
* Validation support using FluentValidation
* Refactoring and fixed all the warnings.
* Deploying to Azure App Services - Online Demos.
	- [REST API - Online Demo](https://minimalapi-demo.azurewebsites.net/swagger/index.html)
	- [GraphQL - Online Demo](https://minimalapi-demo.azurewebsites.net/graphql)
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
