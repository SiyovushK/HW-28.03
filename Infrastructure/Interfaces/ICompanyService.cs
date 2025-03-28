using Domain.Entities;
using Domain.Responses;

namespace Infrastructure.Interfaces;

public interface ICompanyService
{
    Task<Response<List<Company>>> GetAllAsync();

    Task<Response<Company>> GetByIdAsync(int ID);

    Task<Response<string>> CreateAsync(Company company);

    Task<Response<string>> UpdateAsync(Company company);

    Task<Response<string>> DeleteAsync(int ID);
}