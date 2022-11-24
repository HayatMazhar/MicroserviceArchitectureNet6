using Baseline.Web.Models;
using Baseline.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Baseline.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ICouponService _couponService;
        protected new ResponseDto Response;
        

        public CartController(IProductService productService, ICartService cartService, ICouponService couponService)
        {
            _cartService = cartService;
            _couponService = couponService;
        }

        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartDtoBasedOnLoggedInUser());
        }

        [HttpPost]
        [ActionName("ApplyCoupon")]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            Response = await _cartService.ApplyCoupon<ResponseDto>(cartDto, accessToken);

            if (Response is { IsSuccess: true })
            {
                return RedirectToAction(nameof(CartIndex));
            }
            
            return View();
        }

        [HttpPost]
        [ActionName("RemoveCoupon")]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            var user = cartDto.CartHeader.UserId;

            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            Response = await _cartService.RemoveCoupon<ResponseDto>(user, accessToken);

            if (Response != null && Response.IsSuccess)
            {
                return RedirectToAction(nameof(CartIndex));
            }

            return View();
        }

        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            Response = await _cartService.RemoveFromCartAsync<ResponseDto>(cartDetailsId, accessToken);

            if (Response is { IsSuccess: true })
            {
                return RedirectToAction(nameof(CartIndex));
            }

            return View("CartIndex");
        }

        private async Task<CartDto> LoadCartDtoBasedOnLoggedInUser()
        {
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            Response = await _cartService.GetCartByUserIdAsync<ResponseDto>(userId, accessToken);

            CartDto cartDto = new();

            if (Response is { IsSuccess: true })
            {
                cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(Response.Data)!);
            }

            if (cartDto is not { CartHeader: { } }) return cartDto;
           
            if (!string.IsNullOrEmpty(cartDto.CartHeader.CouponCode))
            {
                var coupon = await _couponService.GetCoupon<ResponseDto>(cartDto.CartHeader.CouponCode, accessToken);
                
                if(coupon is { IsSuccess: true })
                {
                    var couponObj = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(coupon.Data)!);
                    cartDto.CartHeader.DiscountTotal = couponObj?.DiscountAmount ?? 0;
                }  
            }

            foreach (var detail in cartDto.CartDetails)
            {
                cartDto.CartHeader.OrderTotal += (detail.Product.ProductPrice * detail.ProductCount);
            }

            if (cartDto.CartHeader.OrderTotal > cartDto.CartHeader.DiscountTotal)
            {
                cartDto.CartHeader.OrderTotal -= cartDto.CartHeader.DiscountTotal;
            }
            return cartDto;
        }
               
        public async Task<IActionResult> Checkout()
        {
            return View(await LoadCartDtoBasedOnLoggedInUser());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CartDto cartDto)
        {
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                Response = await _cartService.Checkout<ResponseDto>(cartDto.CartHeader, accessToken);
                return RedirectToAction(nameof(Confirmation));
            }
            catch (Exception)
            {
                return View(cartDto);                   
            }
        }

        public Task<IActionResult> Confirmation()
        {
            return Task.FromResult<IActionResult>(View());
        }
    }
}
