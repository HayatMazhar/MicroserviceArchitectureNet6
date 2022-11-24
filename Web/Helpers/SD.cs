namespace Baseline.Web.Helpers
{
    public static class SD
    {
        public static string ProductApiBase { get; set; }
        public static string ShoppingCartApiBase { get; set; }
        public static string CouponApiBase { get; set; }

        public enum ApiType
        {
            Get,
            Post,
            Put,
            Delete,
        }
    }
}
