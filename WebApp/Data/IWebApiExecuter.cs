namespace WebApp.Data
{
    public interface IWebApiExecuter
    {
        Task<T?> InVokePost<T>(string relativeUrl, T obj);
        Task<T?> InvokeGet<T>(string relativeUrl);
        Task InvokePut<T>(string relativeUrl, T obj);
    }
}