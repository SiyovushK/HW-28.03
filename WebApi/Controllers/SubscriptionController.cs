using Domain.Entities;
using Domain.Responses;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;
    public SubscriptionController(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }
    
    [HttpGet]
    public async Task<Response<List<Subscription>>> GetCompanySubscriptionsAsync(int companyId)
    {
        return await _subscriptionService.GetCompanySubscriptionsAsync(companyId);
    }

    [HttpPost]
    public async Task<Response<string>> CreateSubscriptionAsync(Subscription subscription)
    {
        return await _subscriptionService.CreateSubscriptionAsync(subscription);
    }

    [HttpPut]
    public async Task<Response<string>> UpdateSubscriptionStatusAsync(int id, bool isActive)
    {
        return await _subscriptionService.UpdateSubscriptionStatusAsync(id, isActive);
    }

    [HttpGet("ActiveSubscriptions")]
    public async Task<Response<List<Subscription>>> GetActiveSubscriptionsAsync()
    {
        return await _subscriptionService.GetActiveSubscriptionsAsync();
    }   
}