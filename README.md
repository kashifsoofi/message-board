# Messages API
This is a bare bone REST API to store messages for users.

This is built using ASP.NET Core 2.2 Web API and can be build either user Visual Studio (Windows/Mac) or dotnet command line tools.

Solution consists of following projects
1. MessageBoard.Api - WebApi project
2. MessageBoard.Api.UnitTests - Unit tests for MessageBoard.Api, this tests individual units part of the funcationality
  Test Framework - xUnit
  Assertions - Shoudly
  Mock - Moq
  TestData - AutoFixture
3. MessageBoard.Api.AcceptanceTests - AcceptanceTest for MessageBoard.Api - this tests by loading the WebApi project in TestHost and calling the endpoints, helps testing Startup that remains untested otherwise
  Test Framework - xUnit
  Assertions - Shoudly
  TestData - AutoFixture
  Microsoft.AspNetCore.TestHost

# Things not implemented
This is a barebone solution so there are a number of things not implemented.
1. Logging
2. Security - hosted as http
3. Input validation
4. Swagger documentation

The solution is kept to a minimum and only implements what is required perhaps to an extreme :)
