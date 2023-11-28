using Course.Shared.Dtos;

namespace Course.Discount.Services;

public interface IDiscountService
{
    Task<Response<List<Models.Discount>>> GetList();
    Task<Response<Models.Discount>> GetById(int id);
    Task<Response<NoContentDto>> Add(Models.Discount discount);
    Task<Response<NoContentDto>> Update(Models.Discount discount);
    Task<Response<NoContentDto>> Delete(int id);
    Task<Response<Models.Discount>> GetByCode(string code, string userId);
}
