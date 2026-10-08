-- Original Problem:
-- The original query performed a SELECT * on Orders, Customers, and OrderItems without any filtering, pagination, or specific column selection.
-- This caused huge memory overhead, table scans, and returned data the API did not need.

-- Optimization Applied:
-- 1. Selected only required columns to reduce bandwidth and memory.
-- 2. Added INNER JOIN specifically for required Customer details.
-- 3. Filtered by OrderDate to limit the initial dataset.
-- 4. Used ORDER BY and OFFSET/FETCH for pagination to prevent loading excessive records.
-- 5. Indexes on OrderDate and CustomerId will make this seek much faster.

-- Expected performance improvement:
-- Query execution time will drop significantly because less data is scanned, less data is transmitted, and indexes prevent full table scans.

SELECT 
    o.OrderId, 
    c.Name AS CustomerName, 
    o.OrderDate, 
    o.Status, 
    o.TotalAmount
FROM 
    Orders o
INNER JOIN 
    Customers c ON o.CustomerId = c.CustomerId
WHERE 
    o.OrderDate >= '2023-01-01'
ORDER BY 
    o.OrderDate DESC
OFFSET 0 ROWS FETCH NEXT 20 ROWS ONLY;
