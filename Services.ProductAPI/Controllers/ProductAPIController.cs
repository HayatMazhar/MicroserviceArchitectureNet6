using Baseline.Services.ProductAPI.Models.Dto;
using Baseline.Services.ProductAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Baseline.Services.ProductAPI.Controllers
{
    [Route("api/products")]
    public class ProductApiController : ControllerBase
    {
        protected ResponseDto ResponseDto;
        private readonly IProductRepository _productRepository;

        public ProductApiController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
            this.ResponseDto = new ResponseDto();
        }

        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                var productRequestDtos = await _productRepository.GetAllProductsByDapper();
                ResponseDto.Data = productRequestDtos;
                ResponseDto.IsSuccess = true;

            }
            catch (Exception ex)
            {
                ResponseDto.IsSuccess = false;
                ResponseDto.Error = new List<ErrorResponseDto> { new() { Code = "500", Text = ex.ToString() } };
            }
            return ResponseDto;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<object> Get(int id)
        {
            try
            {
                ProductRequestDto productRequestDto = await _productRepository.GetProductById(id);
                ResponseDto.Data = productRequestDto;
                ResponseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                ResponseDto.IsSuccess = false;
                ResponseDto.Error = new List<ErrorResponseDto> { new() { Code = "500", Text = ex.ToString() } };
            }

            return ResponseDto;
        }

        [HttpPost]
        [Authorize]
        public async Task<object> Post([FromBody] ProductRequestDto productRequestDto)
        {
            try
            {
                ProductRequestDto productRequestDtoModel = await _productRepository.CreateUpdateProduct(productRequestDto);
                ResponseDto.Data = productRequestDtoModel;
                ResponseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                ResponseDto.IsSuccess = false;
                ResponseDto.Error = new List<ErrorResponseDto> { new() { Code = "500", Text = ex.ToString() } };
            }

            return ResponseDto;
        }

        [HttpPut]
        [Authorize]
        public async Task<object> Put([FromBody] ProductRequestDto productRequestDto)
        {
            try
            {
                ProductRequestDto productRequestDtoModel =
                    await _productRepository.CreateUpdateProduct(productRequestDto);
                ResponseDto.Data = productRequestDtoModel;
                ResponseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                ResponseDto.IsSuccess = false;
                ResponseDto.Error = new List<ErrorResponseDto> { new() { Code = "500", Text = ex.ToString() } };
            }

            return ResponseDto;
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("{id}")]
        public async Task<object> Delete(int id)
        {
            try
            {
                bool isSuccessDto = await _productRepository.DeleteProduct(id);
                ResponseDto.Data = isSuccessDto;
                ResponseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                ResponseDto.IsSuccess = false;
                ResponseDto.Error = new List<ErrorResponseDto> { new() { Code = "500", Text = ex.ToString() } };
            }

            return ResponseDto;
        }

    }
}
