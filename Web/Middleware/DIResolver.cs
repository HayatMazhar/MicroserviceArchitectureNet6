using Baseline.Web.Services.IServices;
using Baseline.Web.Services;

namespace Baseline.Web.Middleware
{
    public class DiResolver
    {
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            // Add services to the container.
            builder.Services.AddHttpClient<IProductService, ProductService>();
            builder.Services.AddHttpClient<ICartService, CartService>();
            builder.Services.AddHttpClient<ICouponService, CouponService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<ICouponService, CouponService>();

        }
    }
}