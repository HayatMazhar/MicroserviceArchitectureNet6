using Baseline.Services.CouponAPI.Models.Dto;

namespace Baseline.Services.CouponAPI.Models.Repository
{
    public interface ICouponRepository
    {
        Task<CouponDto> GetCouponByCode(string couponCode);
    }
}
