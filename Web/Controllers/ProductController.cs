using Baseline.Web.Models;
using Baseline.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Baseline.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        protected new ResponseDto Response;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> ProductIndex()
        {
            List<ProductRequestDto> list = new();
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            Response  = await _productService.GetAllProductsAsync<ResponseDto>(accessToken);
            
            if (Response is { IsSuccess: true })
            {
                list = JsonConvert.DeserializeObject<List<ProductRequestDto>>(Convert.ToString(Response.Data)!);
            }
            
            return View(list);
        }

        public IActionResult ProductCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductCreate(ProductRequestDto model)
        {
            if (!ModelState.IsValid) return View(model);
            
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            Response = await _productService.CreateProductAsync<ResponseDto>(model, accessToken);
           
            if (Response != null && Response.IsSuccess)
            {
                return RedirectToAction(nameof(ProductCreate));
            }

            return View(model);
        }

        public async Task<IActionResult> ProductEdit(int productId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            Response = await _productService.GetProductByIdAsync<ResponseDto>(productId, accessToken);
           
            if (Response is { IsSuccess: true })
            {
                ProductRequestDto model = JsonConvert.DeserializeObject<ProductRequestDto>(Convert.ToString(Response.Data)!);
                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductEdit(ProductRequestDto model)
        {
            if (!ModelState.IsValid) return View(model);
            
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            Response = await _productService.UpdateProductAsync<ResponseDto>(model, accessToken);
            
            if (Response is { IsSuccess: true })
            {
                return RedirectToAction(nameof(ProductIndex));
            }

            return View(model);
        }

        public async Task<IActionResult> ProductDelete(int productId)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            Response = await _productService.GetProductByIdAsync<ResponseDto>(productId, accessToken);
            
            if (Response is not { IsSuccess: true }) return NotFound();

            var model = JsonConvert.DeserializeObject<ProductRequestDto>(Convert.ToString(Response.Data)!);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductDelete(ProductRequestDto model)
        {
            if (!ModelState.IsValid) return View(model);
            
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            Response  = await _productService.DeleteProductAsync<ResponseDto>(model.ProductId, accessToken);
            
            if (Response.IsSuccess)
            {
                return RedirectToAction(nameof(ProductIndex));
            }
            return View(model);
        }
    }
}