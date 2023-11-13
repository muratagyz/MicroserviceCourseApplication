using Course.Services.Catalog.Dtos;
using Course.Shared.Dtos;

namespace Course.Services.Catalog.Services;

public interface ICourseService
{
    public Task<Response<List<CourseDto>>> GetAllAsync();
    public Task<Response<CourseDto>> GetByIdAsync(string id);
    public Task<Response<List<CourseDto>>> GetAllByUserIdAsync(string userId);
    public Task<Response<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto);
    public Task<Response<NoContentDto>> UpdateAsync(CourseUpdateDto courseUpdateDto);
    public Task<Response<NoContentDto>> DeleteAsync(string id);
}
