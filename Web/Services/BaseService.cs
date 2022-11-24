using System.Net.Http.Headers;
using System.Text;
using Baseline.Web.Globalization;
using Baseline.Web.Helpers;
using Baseline.Web.Models;
using Baseline.Web.Services.IServices;
using Newtonsoft.Json;
using static System.GC;

namespace Baseline.Web.Services;

public class BaseService : IBaseService
{
    public ResponseDto ResponseDtoModel { get; set; }
    public IHttpClientFactory HttpClient { get; set; }
    public BaseService(IHttpClientFactory httpClient)
    {
        ResponseDtoModel = new ResponseDto();
        HttpClient = httpClient;
    }

    public async Task<T> SendAsync<T>(ApiRequest apiRequest)
    {
        try
        {
            var client = HttpClient.CreateClient("BaselineAPI");
            var message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(apiRequest.Url);
            client.DefaultRequestHeaders.Clear();

            if (apiRequest.Data != null)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");
            }

            if (!string.IsNullOrEmpty(apiRequest.AccessToken))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", apiRequest.AccessToken);
            }

            message.Method = apiRequest.ApiType switch
            {
                SD.ApiType.Post => HttpMethod.Post,
                SD.ApiType.Put => HttpMethod.Put,
                SD.ApiType.Delete => HttpMethod.Delete,
                _ => HttpMethod.Get
            };

            var apiResponse = await client.SendAsync(message);

            var apiContent = await apiResponse.Content.ReadAsStringAsync();
            var apiResponseDto = JsonConvert.DeserializeObject<T>(apiContent);
            return apiResponseDto;

        }
        catch (Exception e)
        {
            var dto = new ResponseDto
            {
                Error = new List<ErrorResponseDto> { new() { Code = "500", Text = Convert.ToString(e.Message) } },
                IsSuccess = false
            };

            var res = JsonConvert.SerializeObject(dto);
            var apiResponseDto = JsonConvert.DeserializeObject<T>(res);
            return apiResponseDto;
        }
    }
    public void Dispose()
    {
        SuppressFinalize(this);
    }
}