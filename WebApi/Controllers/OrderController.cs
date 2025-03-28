using Domain.DTOs;
using Domain.Entities;
using Domain.Responses;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("CompanyOrders")]
    public async Task<Response<List<Order>>> GetCompanyOrdersAsync(int companyId)
    {
        return await _orderService.GetCompanyOrdersAsync(companyId);
    }

    [HttpPost]
    public async Task<Response<string>> CreateOrderAsync(Order order)
    {
        return await _orderService .CreateOrderAsync(order);
    }

    [HttpPut]
    public async Task<Response<string>> UpdateOrderStatusAsync(int id, string status)
    {
        return await _orderService.UpdateOrderStatusAsync(id, status);
    }

    [HttpGet("DayStatistics")]
    public async Task<Response<TotalOrdersAndRevenue>> GetDailySummaryAsync(DateTime date)
    {
        return await _orderService.GetDailySummaryAsync(date);
    }

    [HttpGet("CompanyStatistics")]
    public async Task<Response<List<CompanyOrdersAndRevenue>>> GetCompanyOrderStatisticsAsync()
    {
        return await _orderService.GetCompanyOrderStatisticsAsync();
    }
}