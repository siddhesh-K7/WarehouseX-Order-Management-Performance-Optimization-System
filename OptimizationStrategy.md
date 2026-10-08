# Optimization Strategy

## Current Performance Problems
1. **Slow database queries:** The application currently pulls too many columns and rows from the database.
2. **N+1 query problems:** Loading items for orders in a loop instead of eager loading.
3. **Missing indexes:** Queries filtering by `OrderDate` and `CustomerId` scan the entire table.
4. **Synchronous database calls:** Blocking threads, which limits the API's concurrency.
5. **Lack of Pagination:** Returning thousands of records to the client at once.
6. **Poor error handling:** Unhandled exceptions causing potential crashes and bad client responses.

## Optimization Goals
- **Reduce database response time** by introducing pagination and selective projection.
- **Improve API concurrency** using `async` / `await` for all IO-bound operations.
- **Reduce memory consumption** by avoiding `.ToList()` before `.Where()` and using `AsNoTracking()` for read-only queries.
- **Improve scalability** by reducing redundant database calls.
- **Ensure application stability** via centralized exception handling middleware.

## Optimization Strategy

### Database
- **Query optimization:** Use LINQ `Select` to retrieve only fields defined in `OrderDto`.
- **Proper indexes:** Add indexes on `OrderDate`, `CustomerId`, and `Status` via EF Core Fluent API.
- **Pagination:** Implement `.Skip()` and `.Take()` at the DB level.
- **Avoid N+1:** If related entities are needed, use `.Include()`, or project directly to a DTO containing only needed scalar properties.

### Application
- **Async calls:** Use `ToListAsync()` and `FirstOrDefaultAsync()`.
- **Efficient LINQ:** Apply `.Where()` before execution. 
- **Reduce object creation:** Use DTOs instead of full Entity frameworks models to return over the API.

### API
- **Pagination:** Allow `page` and `pageSize` parameters.
- **DTOs:** Return structured data (`OrderDto`) instead of database schemas.
- **HTTP status codes:** Return `201 Created`, `204 NoContent`, `400 BadRequest`, `404 NotFound` properly.

### Monitoring
- **Execution time:** Tracked via simple application logs or built-in .NET diagnostic tools.
- **Database query duration:** Monitored using EF Core query logging.
- **Memory usage / CPU usage:** Can be measured via Task Manager or Application Insights.
