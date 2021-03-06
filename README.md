# Clean Architecture ASP.NET Core Web API 3.1

## Project Use Case

This is a WebAPI Project, implemented with Clean Architecture pattern. The code is very clean and it is very manageable. 
You can add your own models, validations, logic, etc. You can also create MVC project on top of it, or just 
use it as a api, deployed locally or publish on some server and use some front-end framework to show the data.

## Installation

1.  Download the code - Clone the repository or download the zip file

2.  Change the connection string - In the WebAPI project in the ```appsettings.json``` file enter your server name 
    and name of the database(I was working on SQL Server 2014 Management Studio).

3.  Go into the Package Manager Console and type: ```Add-Migration "some name of the migration```"
    <br />
    <br />
    ***!IMPORTANT - Make sure that your WebAPI is Set as Startup Project, and in the Package Manager Console your Default project is                                      CleanArchitectureWebAPI.Infrastructure.Data selected.***
    - this will create folder Migrations in CleanArchitectureWebAPI.Infrastructure.Data with the migration. 
      
4.  When this is done, just type in the Package Manager Console: ```Update-Database```
    - this will create the database with you database name with all the ASP.NET Identity tables and the models.
    
5.  Run the project
    - this will build the project and it will seed the database with some data, just something to work with. 
      (maybe you have to close the SQL Server 2014 Management Studio and start it again)
    - in the WebAPI project it will also be created folder Log with two files. One .json file and other .txt.
      These are for detail logging information.
    
6.  Next step is that you have to create "Admin" to take full experience of the API. 
    - because the API works with roles "Admin" and "User", you have to go first to create roles "Admin" and "User" in dbo.AspNetRoles.
    - you can use this set of commands in SQL Server 2014 Management Studio to create the roles "Admin" and "User".

    ```sql
    USE [yourDatabaseName]
    GO
    INSERT INTO dbo.AspNetRoles(Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (1, 'Admin', 'ADMIN', null)
    INSERT INTO dbo.AspNetRoles(Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (2, 'User', 'USER', null)
    ```
    
7.  Next step is to register some user <br />
    - ***from this point on, your api/project should be running all the time during the register, login and CRUD operations***<br />
    - ***I hosted the api/project locally, on my computer***
    - you can use Postman or Fiddler for registration, or some other program, and provide this json format in the body with your own values.
      (I used Postman)
    - The method is ```Post``` and the url will be ```localhost:port/api/account/register```<br />
      
    ```json
    {
       "Username": "YourUsername",
       "Email": "YourEmail@yahoo.com",
       "Password": "YourPassword@123",
       "ConfirmPassword": "YourPassword@123"
    }
     ```
    - after you get registered, automatically, this user will have the role "User". 
       Go in the SQL Server 2014 Management Studio, your database, table AspNetUserRoles and notice that the RoleId is already 2 for that one user. 
       Just edit the table and change it manually to 1 and press TAB,
       or write this command in SQL
       
    ```sql
    USE [yourDatabaseName]
    GO
    UPDATE [dbo].[AspNetUserRoles] SET RoleId = 1 
    WHERE UserId = 'userId' --here enter the user ID
    ```   
    This user, from then on will be with role of "Admin".

8.  Next step is to Sign In some user
    - url: ```localhost/port/api/account/signin```, and this method is also ```Post``` and you should provide this json format into the body
    ```json
    {
        "Username": "YourUsername",
        "Password": "YourPassword@123"
    }
      ```
    the logging user will be successful and you will recive a JWT Token,<br />
    which you must use for Authorization with the prefix Bearer : YourToken
    ```
    In Postman just select the Auth, and from the dropdown menu select Bearer Token
    ```
    
    Now the commands Post, Create, Edit and Delete will be available, otherwise you will get the:
    ```js 
    401 Unauthorized
    ```
    *(for Get and GetById you dont need no Authorization)*
    <br />
    <br />
    Every other registered user in the future will be with the role "User". To change it to "Admin" just do the previous step. 
    (in the project there is no request that is authorized by "User", so you can use this below some other Http request).<br />
    ```C#
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
    ``` 
## Principle, Patterns and external libraries used.

1. Clean (Onion) Architecture Project with Domain Driven Design and SOLID principles.
2. ASP.NET Core Dependency Injection
3. Repository Pattern
4. Unit Of Work Pattern
5. Automapper
6. Logging with Serilog
7. In Memory Caching
8. Response Caching
9. Swagger
10. JWT Token
11. Unit Testing with Moq

## ToDO

1. There is no business logic, since this is only a concept.
2. The enities don't have foreign keys or any common table

## Notes
1. The solution folder Domian, should be named Domain. And also the name of the Class Library from that folder should be named CleanArchitectureWebAPI.Dom**ai**n, not CleanArchitectureWebAPI.Dom**ia**n.

