using System.Net;

namespace eShop.Utility
{
    public sealed class ApiResponseMessage
    {
        public bool IsSuccessful { get; set; }
        public string Error { get; set; }
        public string Message { get; set; }
        public HttpStatusCode? Code { get; set; }

    }
}
