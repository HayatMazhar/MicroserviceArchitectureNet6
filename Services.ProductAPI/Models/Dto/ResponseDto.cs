namespace Baseline.Services.ProductAPI.Models.Dto
{
    public class ResponseDto
    {
        public bool IsSuccess { get; set; } = true;
        public object Data { get; set; }
        public IEnumerable<ErrorResponseDto> Error { get; set; }

    }
    public class ErrorResponseDto
    {
        public string Code { get; set; }
        public string Text { get; set; } = "";
    }
}
