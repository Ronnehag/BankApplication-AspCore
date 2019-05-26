# BankApplication
The application is built using ASP.NET Core 2.2 and Entity Framework Core. The project is built with clean architecture and using CQRS pattern to communicate between the presentation layer and the application layer, containing the business logic.
The application using Core Identity to handle users with claims Admin & Cashier. The API uses JWTTokens to verify the user, the username/password can be hardcoded through a POST request from Postman to api/auth/login since it's only for testing purposes.
The Token is used to call api/me and get the customer JSON data for the email you used to sign in.

The API uses secret.json to set the SecretKey, Issuer and Audiance. To test the API you can hardcode these values in startup.cs as well as in the api/auth/login action method.

## Getting Started
Use these settings to get the project up and running.

### Prerequisites
You will need the following tools:
- Visual Studio 2017 OR 2019
- .NET Core SDK 2.2

### Setup

Follow these steps to get your development environment set up:

1. Clone the repository
2. At the root directory, restore required packages by running "dotnet restore".
3. Build the solution using "dotnet build".
4. Run the program, it will launch in your https://localhost:port/


### Technologies
.NET Core 2.2
ASP.NET Core 2.2
Entity Framework Core 2.2


### Notes
The repository lacks commits because I've developed the application using Azure DevOps. This is just an upload to my Github.
