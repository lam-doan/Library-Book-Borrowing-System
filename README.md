# Library Book Borrowing System (Backend API)

Backend API for managing books, members, and borrowing/returning books using ASP.NET Core and Entity Framework Core.

## Tech Stack
- ASP.NET Core Web API
- Entity Framework Core (SQLite)
- Swagger/OpenAPI
- Layered architecture: Controller -> Service -> Repository

## Features
- Book management
  - Create, list, get by id, update, delete
- Member management
  - Create, list, get by id, update, delete
- Borrowing management
  - Borrow a book
  - Return a book
  - View all borrow records
  - View member borrow history
- Validation and consistent error format
  - `{ "error": "..." }`
- Concurrency-safe borrow flow for last available copy

## Prerequisites
- .NET SDK 10.0+

## Run Locally
From the project root:

```bash
dotnet restore LibraryBookBorrowingSystem.csproj
dotnet run --project LibraryBookBorrowingSystem.csproj --launch-profile http
```

Default local URL:
- `http://localhost:5121`

Swagger UI:
- `http://localhost:5121/swagger`

## API Routes
- `GET /api/books`
- `POST /api/books`
- `GET /api/books/{id}`
- `PUT /api/books/{id}`
- `DELETE /api/books/{id}`
- `GET /api/members`
- `POST /api/members`
- `GET /api/members/{id}`
- `PUT /api/members/{id}`
- `DELETE /api/members/{id}`
- `GET /api/members/{id}/borrows`
- `GET /api/borrows`
- `POST /api/borrows`
- `POST /api/borrows/return`

## Notes
- The project uses a local SQLite database file (`library.db`) configured in `appsettings.json`.
- If you get `NETSDK1045`, install .NET 10 SDK.
