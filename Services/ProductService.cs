using Azure.Core;
using E_Com_Monolithic.Dal;
using E_Com_Monolithic.Dtos;
using E_Com_Monolithic.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using System.Data;

namespace E_Com_Monolithic.Services
{
    public class ProductService
    {
        private readonly ApplicationDbContext _context;
        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> SearchProductBYName(string name)
        {
            var nameParam = new SqlParameter("@name", SqlDbType.NVarChar, 100)
            {
                Value = name
            };
            var products = await _context.products
                .FromSqlRaw("EXECUTE SearchProductNew @name", nameParam)
                .ToListAsync();
            if (products == null || !products.Any())
            {
                return null;
            }
            return products;
        }

        public async Task<List<Product>> SearchProductBYCatogoryName(string categoryName)
        {
            var parameters = new[]
            {
            new SqlParameter("@name", SqlDbType.NVarChar, 100) { Value = DBNull.Value },
            new SqlParameter("@brand", SqlDbType.NVarChar, 100) { Value = DBNull.Value },
            new SqlParameter("@categoryName", SqlDbType.NVarChar, 100) { Value = categoryName },
            new SqlParameter("@minPrice", SqlDbType.Decimal) { Value = DBNull.Value },
            new SqlParameter("@maxPrice", SqlDbType.Decimal) { Value = DBNull.Value }
            };

            var products = await _context.products
           .FromSqlRaw("EXEC SearchProductNew @name, @brand, @categoryName, @minPrice, @maxPrice", parameters)
           .ToListAsync();

            return products;
        }

        public async Task<List<Product>> SearchProductBYCategoryId(int categoryId)
        {
            var parameters = new[]
            {
            new SqlParameter("@name", SqlDbType.NVarChar, 100) { Value = DBNull.Value },
            new SqlParameter("@brand", SqlDbType.NVarChar, 100) { Value = DBNull.Value },
            new SqlParameter("@categoryId", SqlDbType.Int, 100) { Value = categoryId },
            new SqlParameter("@minPrice", SqlDbType.Decimal) { Value = DBNull.Value },
            new SqlParameter("@maxPrice", SqlDbType.Decimal) { Value = DBNull.Value }
            };



            var products = await _context.products
            .FromSqlRaw("EXEC SearchProduct @name, @brand, @categoryId, @minPrice, @maxPrice", parameters)
            .ToListAsync();
            return products;
        }

        public async Task<List<Product>> SearchProductsUniversal(ProductSearchRequestDto dto)
        {
            var parameters = new[]
           {
                new SqlParameter("@searchTerm", SqlDbType.NVarChar, 100)
                {
                    Value = string.IsNullOrWhiteSpace(dto.SearchTerm) ? DBNull.Value : dto.SearchTerm
                },
                new SqlParameter("@name", SqlDbType.NVarChar, 100)
                {
                    Value = string.IsNullOrWhiteSpace(dto.Name) ? DBNull.Value : dto.Name
                },
                new SqlParameter("@categoryName", SqlDbType.NVarChar, 100)
                {
                    Value = string.IsNullOrWhiteSpace(dto.CategoryName) ? DBNull.Value : dto.CategoryName
                },
                new SqlParameter("@description", SqlDbType.NVarChar, 100)
                {
                    Value = string.IsNullOrWhiteSpace(dto.Description) ? DBNull.Value : dto.Description
                },
                new SqlParameter("@brand", SqlDbType.NVarChar, 100)
                {
                    Value = string.IsNullOrWhiteSpace(dto.Brand) ? DBNull.Value : dto.Brand
                },
                new SqlParameter("@minPrice", SqlDbType.Decimal)
                {
                    Value = dto.MinPrice.HasValue ? (object)dto.MinPrice.Value : DBNull.Value,
                    Precision = 10,
                    Scale = 2
                },
                new SqlParameter("@maxPrice", SqlDbType.Decimal)
                {
                    Value = dto.MaxPrice.HasValue ? (object)dto.MaxPrice.Value : DBNull.Value,
                    Precision = 10,
                    Scale = 2
                },
                new SqlParameter("@sortBy", SqlDbType.NVarChar, 50)
                {
                    Value = dto.SortBy ?? "CreatedAt"
                },
                new SqlParameter("@sortOrder", SqlDbType.NVarChar, 50)
                {
                    Value = dto.SortOrder ?? "DESC"
                }
            };

            var sql = @"
        EXEC SearchProductUniversal 
            @searchTerm, 
            @name, 
            @categoryName, 
            @description, 
            @brand, 
            @minPrice, 
            @maxPrice, 
            @sortBy, 
            @sortOrder";

            var products = await _context.products
                .FromSqlRaw(sql, parameters)
                .ToListAsync();

            return products;
        }


    }
}
        