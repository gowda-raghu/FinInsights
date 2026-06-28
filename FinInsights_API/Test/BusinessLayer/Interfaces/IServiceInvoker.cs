using System.Text;
using System.Text.Json;

public interface IServiceInvoker
{
    Task<TResponse?> PostAsync<TRequest,TResponse>(
        string url,
        TRequest request);

    Task<TResponse?> GetAsync<TResponse>(
        string url);
}