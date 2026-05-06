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
            return await httpClient.GetFromJsonAsync<T>(relativeUrl);
        }
        public async Task<T?> InVokePost<T>(string relativeUrl,T obj)
        {
            var httpClient = httpClientFactory.CreateClient(apiName);
            var responseMessage =await httpClient.PostAsJsonAsync(relativeUrl, obj);
            responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadFromJsonAsync<T>();
        }
        public async Task InvokePut<T>(string relativeUrl,T obj)
        {
            var httpClient = httpClientFactory.CreateClient(apiName);
            var response = await httpClient.PutAsJsonAsync(relativeUrl, obj);
            response.EnsureSuccessStatusCode();
        }
    }
}
