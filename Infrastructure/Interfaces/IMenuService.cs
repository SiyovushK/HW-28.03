using Domain.Entities;
using Domain.Responses;

namespace Infrastructure.Interfaces;

public interface IMenuService
{
    Task<Response<Menu>> GetActiveMenuAsync();

    Task<Response<Menu>> GetMenuByDateAsync(DateTime date);

    Task<Response<string>> CreateMenuAsync(Menu request);

    Task<Response<string>> AddMenuItemAsync(MenuItem item);

    Task<Response<List<string>>> GetMenuCategoriesAsync();
}