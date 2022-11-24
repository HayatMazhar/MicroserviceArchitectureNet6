using Baseline.Services.ShoppingCartAPI.Messages;
using Baseline.Services.ShoppingCartAPI.Models.Dto;
using Baseline.Services.ShoppingCartAPI.Models.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Baseline.Services.ShoppingCartAPI.Controllers
{
    [ApiController]
    [Route("api/cart/")]
    public class CartApiController : Controller
    {
        private readonly ICartRepository _cartRepository;
        protected new ResponseDto Response;


        public CartApiController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
            Response = new ResponseDto();
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<object> GetCart(string userId)
        {
            try
            {
                var cartDto = await _cartRepository.GetCartByUserId(userId);
                Response.Data = cartDto;
            }
            catch (Exception ex)
            {
                Response.IsSuccess = false;
                Response.Error = new List<ErrorResponseDto> { new() { Code = "500", Text = ex.ToString() } };
            }
            return Response;
        }

        [HttpPost("AddUpdateCart")]
        public async Task<object> AddUpdateCart(CartDto cart)
        {
            try
            {
                var cartDto = await _cartRepository.CreateUpdateCart(cart);
                Response.Data = cartDto;
            }
            catch (Exception ex)
            {
                Response.IsSuccess = false;
                Response.Error = new List<ErrorResponseDto> { new() { Code = "500", Text = ex.ToString() } };
            }
            return Response;
        }

        [HttpPost("RemoveCart")]
        public async Task<object> RemoveCart([FromBody] int cartId)
        {
            try
            {
                var isSuccess = await _cartRepository.RemoveFromCart(cartId);
                Response.Data = isSuccess;
            }
            catch (Exception ex)
            {
                Response.IsSuccess = false;
                Response.Error = new List<ErrorResponseDto> { new() { Code = "500", Text = ex.ToString() } };
            }
            return Response;
        }

        [HttpPost("ClearCart")]
        public async Task<object> ClearCart([FromBody] string cartId)
        {
            try
            {
                var isSuccess = await _cartRepository.ClearCart(cartId);
                Response.Data = isSuccess;
            }
            catch (Exception ex)
            {
                Response.IsSuccess = false;
                Response.Error = new List<ErrorResponseDto> { new() { Code = "500", Text = ex.ToString() } };
            }
            return Response;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<object> ApplyCoupon([FromBody] CartDto cartDto)
        {
            var user = cartDto.CartHeader.UserId;
            var coupon = cartDto.CartHeader.CouponCode;
            try
            {
                var isSuccess = await _cartRepository.ApplyCoupon(user, coupon);
                Response.Data = isSuccess;
            }
            catch (Exception ex)
            {
                Response.IsSuccess = false;
                Response.Error = new List<ErrorResponseDto> { new() { Code = "500", Text = ex.ToString() } };
            }
            return Response;
        }

        [HttpPost("RemoveCoupon")]
        public async Task<object> RemoveCoupon([FromBody] string userId)
        {
            try
            {
                var isSuccess = await _cartRepository.RemoveCoupon(userId);
                Response.Data = isSuccess;
            }
            
            catch (Exception ex)
            {
                Response.IsSuccess = false;
                Response.Error = new List<ErrorResponseDto> { new() { Code = "500", Text = ex.ToString() } };
            }
            return Response;
        }

        [HttpPost("Checkout")]
        public async Task<object> Checkout(CheckoutHeaderDto checkoutHeader)
        {
            try
            {
                var cartDto = await _cartRepository.GetCartByUserId(checkoutHeader.UserId);

                if (cartDto == null) return BadRequest();

                checkoutHeader.CartDetails = cartDto.CartDetails;

                //TODO: logic to add message to process order
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
