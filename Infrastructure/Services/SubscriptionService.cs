using Domain.Entities;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Dapper;
using System.Net;

namespace Infrastructure.Services;

public class SubscriptionService : ISubscriptionService
{
    DataContext _context = new();

    public async Task<Response<List<Subscription>>> GetCompanySubscriptionsAsync(int companyId)
    {
        using var connection = await _context.GetConnectionAsync();
        var sql = @"
            SELECT * FROM subscriptions 
            WHERE companyId = @CompanyId
        ";
        var result = await connection.QueryAsync<Subscription>(sql, new { CompanyId = companyId });

        return new Response<List<Subscription>>(result.ToList());
    }

    public async Task<Response<string>> CreateSubscriptionAsync(Subscription subscription)
    {
        using var connection = await _context.GetConnectionAsync();
        var sql = @"
            INSERT INTO subscriptions(companyId, planType, mealsPerDay, price, startDate, endDate, isActive, createdAt, updatedAt) 
            VALUES 
            (@CompanyId, @PlanType, @MealsPerDay, @Price, @StartDate, @EndDate, @IsActive, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)
        ";
        var result = await connection.ExecuteAsync(sql, subscription);

        return result == 0 
            ? new Response<string>(HttpStatusCode.BadRequest, "Something went wrong")
            : new Response<string>("Subscription created successfully");
    }

    public async Task<Response<string>> UpdateSubscriptionStatusAsync(int id, bool isActive)
    {
        using var connection = await _context.GetConnectionAsync();
        var sql = @"
            UPDATE subscriptions 
            SET isActive = @IsActive, updatedAt = CURRENT_TIMESTAMP 
            WHERE id = @Id";
        var result = await connection.ExecuteAsync(sql, new { Id = id, IsActive = isActive });

        return result == 0 
            ? new Response<string>(HttpStatusCode.BadRequest, "Something went wrong")
            : new Response<string>("Subscription status updated successfully");
    }

    public async Task<Response<List<Subscription>>> GetActiveSubscriptionsAsync()
    {
        using var connection = await _context.GetConnectionAsync();
        var sql = "SELECT * FROM subscriptions WHERE isActive = true";
        var result = await connection.QueryAsync<Subscription>(sql);

        return new Response<List<Subscription>>(result.ToList());
    }

}