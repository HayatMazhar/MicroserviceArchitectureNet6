using System.Data;
using AutoMapper;
using Baseline.Services.ProductAPI.DbContexts;
using Baseline.Services.ProductAPI.Models;
using Baseline.Services.ProductAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Dapper;

namespace Baseline.Services.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly string _connectionString;


        public ProductRepository(IMapper mapper, ApplicationDbContext db, IConfiguration config) 
        {
            _mapper = mapper;
            _db = db;
            _connectionString = config.GetConnectionString("DefaultConnection");
        }
        public async Task<ProductRequestDto> CreateUpdateProduct(ProductRequestDto productRequestDto)
        {
            Product product = _mapper.Map<ProductRequestDto, Product>(productRequestDto);

            if (product.ProductId > 0)
            {
                _db.Products.Update(product);
            }
            else
            {
                _db.Products.Add(product);
            }

            await _db.SaveChangesAsync();
            return _mapper.Map<Product, ProductRequestDto>(product);
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            try
            {
                Product product = await _db.Products.SingleOrDefaultAsync(x => x.ProductId == productId);

                if (product == null)
                {
                    return false;
                }

                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<IEnumerable<ProductRequestDto>> GetAllProducts()
        {
            List<Product> productsList = await _db.Products.ToListAsync();
            return _mapper.Map<List<ProductRequestDto>>(productsList);
        }

        public async Task<IEnumerable<ProductRequestDto>> GetAllProductsByDapper()
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            const string query = "SELECT * FROM PRODUCTS";
            var products = await db.QueryAsync<Product>(query);
            return _mapper.Map<List<ProductRequestDto>>(products);

        }

        public async Task<ProductRequestDto> GetProductById(int productId)
        {
            Product product = await _db.Products.SingleOrDefaultAsync(x => x.ProductId == productId);
            return _mapper.Map<ProductRequestDto>(product);
        }
    }
}
