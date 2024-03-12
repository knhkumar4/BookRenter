


-----
# Project Overview
The BookRenter application is under development, aiming to provide a platform for renting books. As a developer, my role involves developing RESTful APIs for specific functionalities.

**Endpoint Details**

- **POST /Auth/login:** Provides authentication for users to log in.
- **GET /api/Book/{id}:** Retrieves information about a specific book by its ID.
- **PUT /api/Book/{id}:** Updates information about a specific book.
- **GET /api/SearchBook:** Allows users to search for books based on a search term.
- **GET /api/Book:** Retrieves a list of all available books.
- **POST /api/Book:** Allows the addition of a new book to the system.
- **GET /api/Cart:** Retrieves the list of items in the user's shopping cart.
- **POST /api/Cart:** Adds a book to the user's shopping cart.
- **DELETE /api/Cart/{bookId}:** Removes a specific book from the user's shopping cart.
- **GET /api/Inventory/{bookId}:** Retrieves inventory information for a specific book.
- **GET /api/Inventory:** Retrieves a list of all available inventory items.
- **POST /api/Inventory:** Adds a new inventory item to the system.
- **POST /User:** Allows the registration of a new user(**Just added to testing purpose of different roles)**
-----
**Functionality Details**

- **Search a Book:** Enables users to search for a book by its name or author.
- **Add a Book to Cart:** Allows adding a book to the cart with specific conditions and error handling.
- **Checkout Books in Cart:** Describes the checkout process and actions taken upon successful checkout.
-----
**Unit Testing**

Additionally, thorough testing coverage has been ensured by implementing unit tests for the APIs, repositories, and services using xUnit. These tests validate the functionality and behaviour of the system components, ensuring robustness and reliability throughout the development process.

-----
**Technologies Used**

- ASP.NET 8 WebApi
- .NET 8
- MSSQL
- Jwt-based authentication and role-based authorization
- Default logging by dotnet
- Swagger OpenAPI
- Health Checks
-----
**Features**

- Clean Code Architecture
- RESTful Design
- Entity Framework Core - Code First
- Repository Pattern - Generic
- Unit of Work
- Identity with JWT Authentication
- Role-based Authorization
- Custom Exception Handling Middlewares
- Swagger UI
- Operator Overloading (Instead of AutoMapper)
- Fluent Validation
- Unit Testing
-----
**Project Structure**

- Middlewares
- Controllers
- Services
- Repositories
- Entity
- Tests
-----
# To set up and run the BookRenter project, follow these steps:
1. **Clone the Project:**
   1. Clone the BookRenter project repository from GitHub to your local machine.
      Clone the BookRenter project repository from the GitHub to your local machine.
      url: <https://github.com/knhkumar4/BookRenter/tree/master>
      You can find the project on GitHub at the following URL: [BookRenter GitHub Repository](https://github.com/knhkumar4/BookRenter/tree/master).  

1. **Open the Project:**
   1. Open the project directory in your preferred Integrated Development Environment (IDE) like Visual Studio or Visual Studio Code.
1. **Update Connection String:**
   1. Navigate to the "appsettings.json" file in the project directory.
   1. Locate the "ConnectionStrings" section.
   1. Replace the existing connection string with the provided connection string:

"ConnectionStrings": { "BookRenterDatabase": "Server=LAPTOP-3KRDMGLE;Database=BookRenterDb;User Id=sa;Password=Admin@123;Encrypt=True;TrustServerCertificate=True;" } 

1. Replace **<Your\_Server\_Name>** with your SQL Server instance name.
1. **Execute the SQL Script:**
   1. Open your preferred SQL Server management tool (e.g., SQL Server Management Studio).
   1. Copy the entire SQL script provided for database setup.
      ![](Aspose.Words.d6e9561d-c358-44b8-ac68-7655b234e1de.001.png)
   1. Execute the script in the management tool to create the database schema, populate it with initial data, and establish foreign key constraints.
   1. Ensure that the script runs successfully without any errors.
   1. ` `The entire database setup script, including table creation, initial data insertion, and foreign key constraints, is **executed without adding Entity Framework migration due to the complexity involved in seeding the data.So that I would be easy for you to run the project.**
1. **Run the Project:**
   1. Build the solution in your IDE to ensure all dependencies are resolved.
   1. Start the BookRenter application by running the project from your IDE.
   1. Verify that the application starts without any errors.
1. **Test Endpoints with Postman:**
   1. Import the provided Postman request JSON file into your Postman application.

      ![](Aspose.Words.d6e9561d-c358-44b8-ac68-7655b234e1de.002.png)

   1. Use the imported requests to test various endpoints of the BookRenter application.
   1. Verify that the endpoints function correctly and return expected responses.

I've set up two types of users: admins and regular users. Admins have full access to everything in the app, while regular users can only do certain things, like searching for books, managing their cart, and checking out. 


{  "username": "admin",

`  `"password": "test"}

{  "username": "user",

`  `"password": "user"}



By following these steps, you'll successfully set up and run the BookRenter project, execute the database script, and configure the connection string for database access. Additionally, you'll be able to test the endpoints using the provided Postman requests.
![](Aspose.Words.d6e9561d-c358-44b8-ac68-7655b234e1de.003.png)

![](Aspose.Words.d6e9561d-c358-44b8-ac68-7655b234e1de.004.png)


