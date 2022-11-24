using Baseline.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Baseline.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;

namespace Baseline.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        protected new ResponseDto Response;
        
        public HomeController(ILogger<HomeController> logger, IProductService productService, ICartService cartService)
        {
            _logger = logger;
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductRequestDto> list = new();
            Response = await _productService.GetAllProductsAsync<ResponseDto>("");
            if (Response != null && Response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductRequestDto>>(Convert.ToString(Response.Data)!);
            }
            return View(list);
        }

        [Authorize]
        public async Task<IActionResult> Details(int productId)
        {
            ProductRequestDto model = new();
            var response = await _productService.GetProductByIdAsync<ResponseDto>(productId, " ");
            if (response is { IsSuccess: true })
            {
                model = JsonConvert.DeserializeObject<ProductRequestDto>(Convert.ToString(response.Data)!);
            }
            return View(model);
        }

        [HttpPost]
        [ActionName("Details")]
        [Authorize]
        public async Task<IActionResult> DetailsPost(ProductRequestDto productDto)
        {
            CartDto cartDto = new()
            {
                CartHeader = new CartHeaderDto()
                {
                    UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value
                }
            };

            CartDetailsDto cartDetails = new()
            {
                ProductCount = productDto.ProductCount,
                ProductId = productDto.ProductId,
            };


            Response = await _productService.GetProductByIdAsync<ResponseDto>(productDto.ProductId, " ");

            if (Response != null && Response.IsSuccess)
            {
                cartDetails.Product = JsonConvert.DeserializeObject<ProductRequestDto>(Convert.ToString(Response.Data)!);
            }

            List<CartDetailsDto> cartDetailsDtos = new();
            cartDetailsDtos.Add(cartDetails);
            cartDto.CartDetails = cartDetailsDtos;

            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var addToCartResp = await _cartService.AddUpdateToCart<ResponseDto>(cartDto, accessToken);

            if (addToCartResp is { IsSuccess: true })
            {
                return RedirectToAction(nameof(Index));
            }

            return View(productDto);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public IActionResult Login()
        {
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }
    }
}