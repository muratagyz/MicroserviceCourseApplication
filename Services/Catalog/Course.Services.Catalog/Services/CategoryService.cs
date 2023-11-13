using AutoMapper;
using Course.Services.Catalog.Dtos;
using Course.Services.Catalog.Models;
using Course.Services.Catalog.Settings;
using Course.Shared.Dtos;
using MongoDB.Driver;

namespace Course.Services.Catalog.Services;

public class CategoryService : ICategoryService
{
    private readonly IMongoCollection<Category> _categoryCollection;
    private readonly IMapper _mapper;

    public CategoryService(IMapper mapper, IDatabaseSettings databaseSettings)
    {
        var client = new MongoClient(databaseSettings.ConnectionString);
        var database = client.GetDatabase(databaseSettings.DatabaseName);

        _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
        _mapper = mapper;
    }

    public async Task<Response<List<CategoryDto>>> GetAllAsync()
    {
        var categories = await _categoryCollection.Find(category => true).ToListAsync();
        return Response<List<CategoryDto>>.Success(data: _mapper.Map<List<CategoryDto>>(categories), statusCode: 200);
    }

    public async Task<Response<Category>> CreateAsync(CategoryDto categoryDto)
    {
        var category = _mapper.Map<Category>(categoryDto);
        await _categoryCollection.InsertOneAsync(category);
        return Response<Category>.Success(data: category, statusCode: 200);
    }

    public async Task<Response<CategoryDto>> GetByIdAsync(string id)
    {
        var category = await _categoryCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        if (category == null)
        {
            return Response<CategoryDto>.Fail(error: "Category not found", statusCode: 404);
        }

        return Response<CategoryDto>.Success(data: _mapper.Map<CategoryDto>(category), statusCode: 200);
    }
}
