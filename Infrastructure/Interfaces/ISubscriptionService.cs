using Domain.Entities;
using Domain.Responses;

namespace Infrastructure.Interfaces;

public interface ISubscriptionService
{
    Task<Response<List<Subscription>>> GetCompanySubscriptionsAsync(int companyId);

    Task<Response<string>> CreateSubscriptionAsync(Subscription subscription);

    Task<Response<string>> UpdateSubscriptionStatusAsync(int id, bool isActive);

    Task<Response<List<Subscription>>> GetActiveSubscriptionsAsync();
}