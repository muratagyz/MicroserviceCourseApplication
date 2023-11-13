using Course.Services.Catalog.Dtos;
using Course.Services.Catalog.Models;
using Course.Shared.Dtos;

namespace Course.Services.Catalog.Services;

public interface ICategoryService
{
    public Task<Response<List<CategoryDto>>> GetAllAsync();
    public Task<Response<Category>> CreateAsync(CategoryDto categoryDto);
    public Task<Response<CategoryDto>> GetByIdAsync(string id);
}
