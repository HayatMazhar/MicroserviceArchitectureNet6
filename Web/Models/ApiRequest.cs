using static Baseline.Web.Helpers.SD;

namespace Baseline.Web.Models
{
    public class ApiRequest
    {
        public ApiType ApiType { get; set; } = ApiType.Get;
        public string Url { get; set; }
        public object Data { get; set; }
        public string AccessToken { get; set; }
    }
}
