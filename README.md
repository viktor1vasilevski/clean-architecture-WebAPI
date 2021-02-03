# Clean Architecture WebAPI

## Project Use Case

This is a WebAPI, implemented with Clean Architecture pattern. The code is very clean and it is very manageable. 
You can add your own models, validations, etc. You can also create MVC project on top of it, or just 
use it as a api, deployed locally or publish on some server and use some front-end framework to show the data.

## Installation

1.  Change the connection string - In the WebAPI project in the appsettings.json file enter your server name 
    and name of the database(I was working on SQL Server 2014 Management Studio).

2.  Go into the Package Manager Console and type: add-migration 
      !IMPORTANT - Make sure that your WebAPI is Set as Startup Project, 
      and in the Package Manager Console your Default project is CleanArchitectureWebAPI.Infrastructure.Data selected.
    - this will create folder Migration in CleanArchitectureWebAPI.Infrastructure.Data with the migrations. 
      
3.  When this is done, just type in the Package Manager Console: update-database.
    - this will create the database with you database name with all the ASP.NET Identity tables and the models.
    - then just run the project on F5 and this will build the project and it will seed the database with some data, just something to work with. (maybe you have to close the         SQL Server 2014 Management Studio and start it again)
    - in the WebAPI project it will also be created folder Log with two files. One .json file and other .txt.
      These are for detail logging information.
    
4.  Next step is that you have to create "Admin" to take full experience of the API. 
    - because the API works with roles "Admin" and "User", you have to go first to create roles "Admin" and "User" in dbo.AspNetRoles.
    - you can use this set of commands in SQL Server 2014 Management Studio to create the roles "Admin" and "User".
    
    USE {yourDatabaseName}
    GO
    INSERT INTO dbo.AspNetRoles(Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (1, 'Admin', 'ADMIN', null)
    INSERT INTO dbo.AspNetRoles(Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (2, 'User', 'USER', null)
    
5.  Next step is to register some user.
    - you can {url}/swagger here, just to see what input should you provide to get registered. (you can use Postman or Fiddler for registration)
    - after you get registered, automatically this user will have the role "User". 
       Go in the table AspNetUserRoles and notice that the RoleId is already 2 for that one user. Just change it manually to 1 and press TAB.
       Or write this command in SQL
       
       UPDATE [dbo].[AspNetUserRoles] SET RoleId = 1 
       WHERE UserId = '{userId}'
       
       This user, from then on will be with the role of "Admin". 
    
    Every other registered user in the future will be with the role "User". To change it to "Admin" just do the previous step. 
    (in the project there is no request that is authorized by "User", so you can use this:
      [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")] , below some other Http request).

This WebAPI uses JSON Web Token (JWT) for Authorization and Information Exchange.
This WebAPI also uses swagger, so you can test it. Just write in the URL insted of {url}/api/soaps, or {url}/api/balms, just write {url}/swagger.
This WebAPI uses Serilog for activity tracking.

## Principle, Patterns and external libraries used.

1. Clean (Onion) project architecture with Domain Driven Design aand SOLID principles.
2. ASP.NET Core Dependency Injection
3. Repository Pattern
4. Automapper
5. Logging with Serilog
6. In Memory Caching
7. Response Caching
8. Swagger
9. JWT Token 

## ToDO

1. There is no business logic, since this is only a concept.
2. The enitiies don't have foreign keys or any common table
