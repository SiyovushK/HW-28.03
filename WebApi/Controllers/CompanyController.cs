using Domain.Entities;
using Domain.Responses;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;
    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpGet]
    public async Task<Response<List<Company>>> GetAllAsync()
    {
        return await _companyService.GetAllAsync();
    }

    [HttpGet("ByID")]
    public async Task<Response<Company>> GetByIdAsync(int ID)
    {
        return await _companyService.GetByIdAsync(ID);
    }

    [HttpPost]
    public async Task<Response<string>> CreateAsync(Company company)
    {
        return await _companyService.CreateAsync(company);
    }

    [HttpPut]
    public async Task<Response<string>> UpdateAsync(Company company)
    {
        return await _companyService.UpdateAsync(company);
    }

    [HttpDelete]
    public async Task<Response<string>> DeleteAsync(int ID)
    {
        return await _companyService.DeleteAsync(ID);
    }
}