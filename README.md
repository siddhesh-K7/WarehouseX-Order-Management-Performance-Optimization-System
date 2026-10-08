# WarehouseX – Order Management Performance Optimization

## Project Description
WarehouseX is a simplified Order Management System designed to demonstrate practical performance optimization techniques in an ASP.NET Core Web API application. The project showcases how to handle inefficient database queries, synchronous code blocks, and poorly structured API responses by applying modern best practices.

## Technologies
- C# 12 / .NET 8
- ASP.NET Core Web API
- Entity Framework Core (SQLite)
- xUnit & In-Memory Database for testing
- RESTful API design
- LINQ & async/await

## Features
- **Order Management:** Create, read, update, delete, and search orders.
- **Optimized SQL:** Indexes and projected LINQ queries to minimize DB load.
- **Pagination:** Implemented efficiently at the database level.
- **Async Database Access:** 100% asynchronous IO operations.
- **DTOs:** Clean separation of internal models and API payloads.
- **Error Handling:** Centralized exception handling middleware.

## Project Structure
- `WarehouseX/Models/`: Database entity classes.
- `WarehouseX/DTOs/`: Data Transfer Objects for API requests and responses.
- `WarehouseX/Data/`: EF Core DbContext and configurations.
- `WarehouseX/Services/`: Business logic and database operations.
- `WarehouseX/Controllers/`: REST API endpoints.
- `WarehouseX/Middleware/`: Custom exception handling.
- `WarehouseX.Tests/`: Unit tests demonstrating performance behavior.
- `OptimizationStrategy.md`: Strategic plan for performance improvements.
- `OptimizedQuery.sql`: The finalized, highly optimized SQL query.
- `CopilotReflection.md`: Summary of how Microsoft Copilot was utilized.

## Optimization Strategy
Identified slow synchronous calls and large data payloads as the primary bottlenecks. The solution involved implementing `async/await` throughout the stack, adding `.AsNoTracking()` to read operations, using `Select()` for DTO projection, and configuring EF Core to generate paginated SQL queries with appropriate indexes on heavily queried columns.

## SQL Optimization
The original query fetched all columns and related entities without filtering or limits. The final query (`OptimizedQuery.sql`) selectively picks only necessary columns, joins efficiently, filters by `OrderDate`, and uses `OFFSET/FETCH` for pagination, leveraging the newly added DB indexes.

## Application Optimization
- Replaced `.Result` with `await`.
- Added `.AsNoTracking()` to read-only queries to save memory.
- Prevented N+1 queries by projecting directly to DTOs in the initial query.
- Implemented global exception handling to avoid API crashes.

## Debugging
Fixed runtime issues like serialization cycles by introducing DTOs. Resolved NullReferenceExceptions by adding validation checks in the `OrderService` for missing customers and insufficient product stock before persisting orders.

## Testing
Tested using xUnit and the EF Core In-Memory database. Tests verify that paginated queries return the correct number of records and handle missing entities gracefully.

## Performance
- **Query execution time:** Expected to drop by >50% due to index usage and reduced payload size.
- **Memory usage:** Expected to decrease significantly as entities are no longer tracked for read operations and full entity trees are not loaded into memory.

## Microsoft Copilot Contribution
Please see `CopilotReflection.md` for a detailed breakdown of how Microsoft Copilot assisted in planning, coding, and debugging the project.

## How to Run

```bash
cd WarehouseX
dotnet restore
dotnet build
dotnet run
```

To run tests:

```bash
cd WarehouseX.Tests
dotnet test
```
