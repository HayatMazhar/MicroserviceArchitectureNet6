using Baseline.Services.CouponAPI.Models.Dto;
using Baseline.Services.CouponAPI.Models.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Baseline.Services.CouponAPI.Controllers
{
    [ApiController]
    [Route("api/coupon")]
    public class CouponApiController : Controller
    {
        private readonly ICouponRepository _couponRepository;
        protected new ResponseDto Response;

        public CouponApiController(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
            Response = new ResponseDto();
        }

        [HttpGet("{code}")]
        public async Task<object> GetDiscountForCode(string code)
        {
            try
            {
                var coupon = await _couponRepository.GetCouponByCode(code);
                Response.Data = coupon;
            }
            catch (Exception ex)
            {
                Response.IsSuccess = false;
                Response.Error = new List<ErrorResponseDto> { new() { Code = "500", Text = ex.ToString() } };
            }

            return Response;
        }

    }
}
