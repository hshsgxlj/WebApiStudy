using System.Net;
using System.Text.Json;

namespace WebApp.Data
{
    public class WebApiException:Exception
    {
        public ErrorResponse? ErrorResponse { get; set; }
        public WebApiException(string errorjson)
        {
            ErrorResponse = JsonSerializer.Deserialize<ErrorResponse>(errorjson);

        }
    }
}
