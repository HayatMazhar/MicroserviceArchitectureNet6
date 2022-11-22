using AutoMapper;
using Baseline.Services.CouponAPI.DbContexts;
using Baseline.Services.CouponAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Baseline.Services.CouponAPI.Models.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContext _db;
        protected IMapper Mapper;

        public CouponRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            Mapper = mapper;
        }

        public async Task<CouponDto> GetCouponByCode(string couponCode)
        {
            var couponFromDb = await _db.Coupons.FirstOrDefaultAsync(u => u.CouponCode == couponCode);
            return Mapper.Map<CouponDto>(couponFromDb);
        }
    }
}
