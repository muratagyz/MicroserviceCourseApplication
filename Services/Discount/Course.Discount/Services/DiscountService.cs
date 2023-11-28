﻿using Course.Shared.Dtos;
using Dapper;
using Npgsql;
using System.Data;

namespace Course.Discount.Services;

public class DiscountService : IDiscountService
{
    private readonly IConfiguration _configuration;
    private readonly IDbConnection _dbConnection;

    public DiscountService(IConfiguration configuration)
    {
        _configuration = configuration;

        _dbConnection = new NpgsqlConnection(_configuration.GetConnectionString("PostgreSql"));
    }

    public async Task<Response<NoContentDto>> Add(Models.Discount discount)
    {
        var status = await _dbConnection.ExecuteAsync("INSERT INTO discount (userid,rate,code) VALUES (@UserId,@Rate,@Code)", discount);

        if (status > 0)
        {
            return Response<NoContentDto>.Success(204);
        }

        return Response<NoContentDto>.Fail("an error occurred while adding", 500);
    }

    public async Task<Response<NoContentDto>> Delete(int id)
    {
        var status = await _dbConnection.ExecuteAsync("delete from discount where id=@Id", new
        {
            Id = id
        });

        if (status > 0)
        {
            return Response<NoContentDto>.Success(204);
        }

        return Response<NoContentDto>.Fail("Discount not found", 400);
    }

    public async Task<Response<Models.Discount>> GetByCode(string code, string userId)
    {
        var discount = (await _dbConnection.QueryAsync<Models.Discount>("Select * from discount where userid=@UserId and code=@Code", new { Code = code, UserId = userId })).FirstOrDefault();

        if (discount == null)
        {
            return Response<Models.Discount>.Fail("discount not found", 404);
        }

        return Response<Models.Discount>.Success(discount, 200);
    }

    public async Task<Response<Models.Discount>> GetById(int id)
    {
        var discount = (await _dbConnection.QueryAsync<Models.Discount>("Select * from discount where id=@Id", new { Id = id })).SingleOrDefault();

        if (discount == null)
        {
            return Response<Models.Discount>.Fail("discount not found", 404);
        }

        return Response<Models.Discount>.Success(discount, 200);
    }

    public async Task<Response<List<Models.Discount>>> GetList()
    {
        var discounts = await _dbConnection.QueryAsync<Models.Discount>("Select * from discount");

        return Response<List<Models.Discount>>.Success(discounts.ToList(), 200);
    }

    public async Task<Response<NoContentDto>> Update(Models.Discount discount)
    {
        var status = await _dbConnection.ExecuteAsync("update discount set userid=@UserId,code=@Code,rate=@Rate where id=@Id", new
        {
            Id = discount.Id,
            UserId = discount.UserId,
            Code = discount.Code,
            Rate = discount.Rate
        });

        if (status > 0)
        {
            return Response<NoContentDto>.Success(204);
        }

        return Response<NoContentDto>.Fail("Discount not found", 404);
    }
}
