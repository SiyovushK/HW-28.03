using Domain.Entities;
using Domain.Responses;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly IMenuService _menuService;
    public MenuController(IMenuService menuService)
    {
        _menuService = menuService;
    }

    [HttpGet]
    public async Task<Response<Menu>> GetActiveMenuAsync()
    {
        return await _menuService.GetActiveMenuAsync();
    }

    [HttpGet("ByDate")]
    public async Task<Response<Menu>> GetMenuByDateAsync(DateTime date)
    {
        return await _menuService.GetMenuByDateAsync(date);
    }

    [HttpPost]
    public async Task<Response<string>> CreateMenuAsync(Menu menu)
    {
        return await _menuService.CreateMenuAsync(menu);
    }

    [HttpPost("MenuItem")]
    public async Task<Response<string>> AddMenuItemAsync(MenuItem item)
    {
        return await _menuService.AddMenuItemAsync(item);
    }

    [HttpGet("MenuCategories")]
    public async Task<Response<List<string>>> GetMenuCategoriesAsync()
    {
        return await _menuService.GetMenuCategoriesAsync();
    }
}