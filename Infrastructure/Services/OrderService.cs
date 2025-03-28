using System.Net;
using Dapper;
using Domain.DTOs;
using Domain.Entities;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Interfaces;

namespace Infrastructure.Services;

public class OrderService : IOrderService
{
    DataContext _context = new();

    public async Task<Response<List<Order>>> GetCompanyOrdersAsync(int companyId)
    {
        using var connection = await _context.GetConnectionAsync();
        var sql = "SELECT * FROM orders WHERE companyId = @CompanyId";
        var result = await connection.QueryAsync<Order>(sql, new {CompanyId = companyId});

        return result == null
            ? new Response<List<Order>>(HttpStatusCode.NotFound, "No orders found for this company")
            : new Response<List<Order>>(result.ToList());
    }

    public async Task<Response<string>> CreateOrderAsync(Order order)
    {
        using var connection = await _context.GetConnectionAsync();
        var sql = @"
            INSERT INTO orders(companyId, orderDate, status, totalAmount, createdAt, updatedAt)
            VALUES
            (@companyId, @orderDate, @status, @totalAmount, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)
        ";
        var result = await connection.ExecuteAsync(sql, order);

        return result == 0
            ? new Response<string>(HttpStatusCode.BadRequest, "Something went wrong")
            : new Response<string>("Order created successefully");
    }

    public async Task<Response<string>> UpdateOrderStatusAsync(int id, string status)
    {
        using var connection = await _context.GetConnectionAsync();
        var sql = @"
            UPDATE orders 
            SET status = @Status, updatedAt = CURRENT_TIMESTAMP
            WHERE id = @Id
        ";
        var result = await connection.ExecuteAsync(sql, new { Id = id, Status = status });

        return result == 0
            ? new Response<string>(HttpStatusCode.BadRequest, "Something went wrong")
            : new Response<string>("Order status updated successefully");
    }

    public async Task<Response<TotalOrdersAndRevenue>> GetDailySummaryAsync(DateTime date)
    {
        using var connection = await _context.GetConnectionAsync();
        var sql = @"
            SELECT 
                COUNT(*) AS TotalOrders, 
                SUM(totalAmount) AS TotalRevenue
            FROM orders 
            WHERE orderDate = @Date
        ";
        var result = await connection.QueryFirstOrDefaultAsync<TotalOrdersAndRevenue>(sql, new { Date = date });

        return result == null
            ? new Response<TotalOrdersAndRevenue>(HttpStatusCode.NotFound, "No orders found for this date.")
            : new Response<TotalOrdersAndRevenue>(result);
    }

    public async Task<Response<List<CompanyOrdersAndRevenue>>> GetCompanyOrderStatisticsAsync()
    {
        using var connection = await _context.GetConnectionAsync();
        var sql = @"
            SELECT 
                companyId, 
                COUNT(*) AS TotalOrders,
                SUM(totalAmount) AS TotalRevenue
            FROM orders
            GROUP BY companyId
            ORDER BY companyId
        ";
        var statistics = await connection.QueryAsync<CompanyOrdersAndRevenue>(sql);

        return statistics == null
            ? new Response<List<CompanyOrdersAndRevenue>>(HttpStatusCode.NotFound, "No statistics found.")
            : new Response<List<CompanyOrdersAndRevenue>>(statistics.ToList());
    }
}