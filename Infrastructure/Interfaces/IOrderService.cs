using Domain.DTOs;
using Domain.Entities;
using Domain.Responses;

namespace Infrastructure.Interfaces;

public interface IOrderService
{
    Task<Response<List<Order>>> GetCompanyOrdersAsync(int companyId);

    Task<Response<string>> CreateOrderAsync(Order order);

    Task<Response<string>> UpdateOrderStatusAsync(int id, string status);

    Task<Response<TotalOrdersAndRevenue>> GetDailySummaryAsync(DateTime date);

    Task<Response<List<CompanyOrdersAndRevenue>>> GetCompanyOrderStatisticsAsync();
}