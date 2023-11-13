using AutoMapper;
using Course.Services.Catalog.Dtos;
using Course.Services.Catalog.Models;
using Course.Services.Catalog.Settings;
using Course.Shared.Dtos;
using MongoDB.Driver;

namespace Course.Services.Catalog.Services;

public class CourseService: ICourseService
{
    private readonly IMongoCollection<Models.Course> _courseCollection;
    private readonly IMongoCollection<Category> _categoryCollection;
    private readonly IMapper _mapper;

    public CourseService(IMapper mapper, IDatabaseSettings databaseSettings)
    {
        var client = new MongoClient(databaseSettings.ConnectionString);
        var database = client.GetDatabase(databaseSettings.DatabaseName);

        _courseCollection = database.GetCollection<Models.Course>(databaseSettings.CourseCollectionName);
        _categoryCollection = database.GetCollection<Models.Category>(databaseSettings.CategoryCollectionName);
        _mapper = mapper;
    }

    public async Task<Response<List<CourseDto>>> GetAllAsync()
    {
        var courses = await _courseCollection.Find(course => true).ToListAsync();

        if (courses.Any())
        {
            foreach (var course in courses)
            {
                course.Category = await _categoryCollection.Find(x => x.Id == course.CategoryId).FirstAsync();
            }
        }
        else
        {
            courses = new List<Models.Course>();
        }

        return Response<List<CourseDto>>.Success(data: _mapper.Map<List<CourseDto>>(courses), statusCode: 200);
    }

    public async Task<Response<CourseDto>> GetByIdAsync(string id)
    {
        var course = await _courseCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        if (course == null)
        {
            return Response<CourseDto>.Fail(error: "Course not found", statusCode: 200);
        }

        course.Category = await _categoryCollection.Find(x => x.Id == course.CategoryId).FirstAsync();

        return Response<CourseDto>.Success(data: _mapper.Map<CourseDto>(course), statusCode: 200);
    }

    public async Task<Response<List<CourseDto>>> GetAllByUserIdAsync(string userId)
    {
        var courses = await _courseCollection.Find(x => x.UserId == userId).ToListAsync();

        if (courses.Any())
        {
            foreach (var course in courses)
            {
                course.Category = await _categoryCollection.Find(x => x.Id == course.CategoryId).FirstAsync();
            }
        }
        else
        {
            courses = new List<Models.Course>();
        }

        return Response<List<CourseDto>>.Success(data: _mapper.Map<List<CourseDto>>(courses), statusCode: 200);
    }

    public async Task<Response<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto)
    {
        var newCourse = _mapper.Map<Models.Course>(courseCreateDto);

        newCourse.CreatedTime = DateTime.Now;

        await _courseCollection.InsertOneAsync(newCourse);

        return Response<CourseDto>.Success(data: _mapper.Map<CourseDto>(newCourse), statusCode: 200);
    }

    public async Task<Response<NoContentDto>> UpdateAsync(CourseUpdateDto courseUpdateDto)
    {
        var updateCourse = _mapper.Map<Models.Course>(courseUpdateDto);

        var result = await _courseCollection.FindOneAndReplaceAsync(x => x.Id == updateCourse.Id, updateCourse);

        if (result == null)
        {
            return Response<NoContentDto>.Fail(error: "Course not found", statusCode: 404);
        }

        return Response<NoContentDto>.Success(statusCode: 204);
    }

    public async Task<Response<NoContentDto>> DeleteAsync(string id)
    {
        var result = await _courseCollection.DeleteOneAsync(x => x.Id == id);

        if (result.DeletedCount > 0)
        {
            return Response<NoContentDto>.Success(statusCode: 204);
        }

        return Response<NoContentDto>.Fail(error: "Course not found", statusCode: 404);
    }
}
