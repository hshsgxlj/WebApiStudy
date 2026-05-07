namespace WebApp.Data
{
    public class WebApiExecuter : IWebApiExecuter
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly string apiName = "ShirtsApi";

        public WebApiExecuter(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<T?> InvokeGet<T>(string relativeUrl)
        {
            var httpClient = httpClientFactory.CreateClient(apiName);
            var request=new HttpRequestMessage(HttpMethod.Get, relativeUrl);
            var response = await httpClient.SendAsync(request);
            await HandlePotentialError(response);
            return await response.Content.ReadFromJsonAsync<T>();
        }
        public async Task<T?> InvokePost<T>(string relativeUrl,T obj)
        {
            var httpClient = httpClientFactory.CreateClient(apiName);
            var responseMessage =await httpClient.PostAsJsonAsync(relativeUrl, obj);
            await HandlePotentialError(responseMessage);
            return await responseMessage.Content.ReadFromJsonAsync<T>();
        }
        public async Task InvokePut<T>(string relativeUrl,T obj)
        {
            var httpClient = httpClientFactory.CreateClient(apiName);
            var response = await httpClient.PutAsJsonAsync(relativeUrl, obj);
            await HandlePotentialError(response);
        }
        public async Task InvokeDelete<T>(string relativeUrl)
        {
            var httpClient = httpClientFactory.CreateClient(apiName);
            var response = await httpClient.DeleteAsync(relativeUrl);
            await HandlePotentialError(response);
        }
        private async Task HandlePotentialError(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new WebApiException(errorResponse);
            }
        }
    }
}
