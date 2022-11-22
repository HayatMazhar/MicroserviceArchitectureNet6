using Baseline.Web.Models;

namespace Baseline.Web.Services.IServices
{
    public interface IBaseService: IDisposable
    {
        ResponseDto ResponseDtoModel { get; set; }

        Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}
