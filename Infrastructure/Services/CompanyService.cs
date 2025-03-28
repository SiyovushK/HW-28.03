using Domain.Entities;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Dapper;
using System.Net;

namespace Infrastructure.Services;

public class CompanyService : ICompanyService
{
    DataContext _context = new();

    public async Task<Response<List<Company>>> GetAllAsync()
    {
        using var connection = await _context.GetConnectionAsync();
        var sql = "SELECT * FROM companies";
        var result = await connection.QueryAsync<Company>(sql);

        return new Response<List<Company>>(result.ToList());
    }

    public async Task<Response<Company>> GetByIdAsync(int ID)
    {
        using var connection = await _context.GetConnectionAsync();
        var sql = "SELECT * FROM companies WHERE id = @Id";
        var result = await connection.QueryFirstOrDefaultAsync<Company>(sql, new {Id = ID});

        return result == null
            ? new Response<Company>(HttpStatusCode.NotFound, "Not found")
            : new Response<Company>(result);
    }

    public async Task<Response<string>> CreateAsync(Company company)
    {
        using var connection = await _context.GetConnectionAsync();
        var sql = @"
            INSERT INTO companies(name, address, phone, email, createdAt, updatedAt) 
            VALUES 
            (@Name, @Address, @Phone, @Email, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)
        ";
        var result = await connection.ExecuteAsync(sql, company);

        return result == 0
            ? new Response<string>(HttpStatusCode.BadRequest, "Something went wrong")
            : new Response<string>("Company created successefully");
    }

    public async Task<Response<string>> UpdateAsync(Company company)
    {
        using var connection = await _context.GetConnectionAsync();
        var sql = @"
            UPDATE companies 
            SET name = @Name, address = @Address, phone = @Phone, email = @Email, updatedAt = CURRENT_TIMESTAMP
            WHERE id = @Id;
        ";
        var result = await connection.ExecuteAsync(sql, company);

        return result == 0 
            ? new Response<string>(HttpStatusCode.BadRequest, "Something went wrong")
            : new Response<string>("Company updated successfully");
    }

    public async Task<Response<string>> DeleteAsync(int ID)
    {
        using var connection = await _context.GetConnectionAsync();
        var sql = @"
            DELETE FROM companies
            WHERE id = @Id;
        ";
        var result = await connection.ExecuteAsync(sql, new {Id = ID});

        return result == 0 
            ? new Response<string>(HttpStatusCode.BadRequest, "Something went wrong")
            : new Response<string>("Company deleted successfully");
    }
}