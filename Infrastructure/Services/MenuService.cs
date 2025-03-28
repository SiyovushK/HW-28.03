using System.Net;
using Dapper;
using Domain.Entities;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Interfaces;

namespace Infrastructure.Services;

public class MenuService : IMenuService
{
    DataContext _context = new();

    public async Task<Response<Menu>> GetActiveMenuAsync()
    {
        using var connection = await _context.GetConnectionAsync();
        var sql = @"
            SELECT * FROM menus 
            WHERE isActive = true 
            ORDER BY menuDate DESC LIMIT 1
        ";
        var result = await connection.QueryFirstOrDefaultAsync<Menu>(sql);

        return result == null
            ? new Response<Menu>(HttpStatusCode.NotFound, "Active menu not found")
            : new Response<Menu>(result);
    }

    public async Task<Response<Menu>> GetMenuByDateAsync(DateTime date)
    {
        using var connection = await _context.GetConnectionAsync();
        var sql = "SELECT * FROM menus WHERE menuDate = @Date";
        var result = await connection.QueryFirstOrDefaultAsync<Menu>(sql, new { Date = date });

        return result == null
            ? new Response<Menu>(HttpStatusCode.NotFound, "Menu not found for the given date")
            : new Response<Menu>(result);
    }

    public async Task<Response<string>> CreateMenuAsync(Menu menu)
    {
        using var connection = await _context.GetConnectionAsync();
        var sql = @"
            INSERT INTO menus (menuDate, isActive, createdAt, updatedAt) 
            VALUES 
            (@MenuDate, @IsActive, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)
        ";
        var result = await connection.ExecuteAsync(sql, menu);

        return result == 0
            ? new Response<string>(HttpStatusCode.BadRequest, "Something went wrong")
            : new Response<string>("Menu created successfully");
    }

    public async Task<Response<string>> AddMenuItemAsync(MenuItem item)
    {
        using var connection = await _context.GetConnectionAsync();
        var sql = @"
            INSERT INTO MenuItems (menuId, name, description, price, category, createdAt, updatedAt) 
            VALUES 
            (@MenuId, @Name, @Description, @Price, @Category, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)
        ";
        var result = await connection.ExecuteAsync(sql, item);

        return result == 0
            ? new Response<string>(HttpStatusCode.BadRequest, "Something went wrong")
            : new Response<string>("Menu item added successfully");
    }

    public async Task<Response<List<string>>> GetMenuCategoriesAsync()
    {
        using var connection = await _context.GetConnectionAsync();
        var sql = "SELECT DISTINCT(category) FROM MenuItems";
        var result = await connection.QueryAsync<string>(sql);

        return result == null
            ? new Response<List<string>>(HttpStatusCode.NotFound, "No categories found")
            : new Response<List<string>>(result.ToList());
    }
}
