using Polly;
using Polly.Retry;

public class ServiceInvoker : IServiceInvoker
{
    private readonly IHttpClientFactory _clientFactory;

    public ServiceInvoker(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(
    string url,
    TRequest request)
    {
        AsyncRetryPolicy<HttpResponseMessage> retryPolicy =
            Policy<HttpResponseMessage>
                .Handle<HttpRequestException>()
                .OrResult(r =>
                    r.StatusCode == System.Net.HttpStatusCode.BadGateway ||
                    r.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable ||
                    r.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                );

        var _httpClient = _clientFactory.CreateClient("InternalServices");

        var response = await retryPolicy.ExecuteAsync(() =>
            _httpClient.PostAsJsonAsync(url, request));

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();

            throw new Exception(
                $"URL: {url}\n" +
                $"Status Code: {(int)response.StatusCode} ({response.StatusCode})\n" +
                $"Response: {error}");
        }

        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    //     public async Task<TResponse?> PostAsync<TRequest, TResponse>(
    //     string url,
    //     TRequest request)
    // {
    //     var client = _clientFactory.CreateClient("InternalServices");

    //     var response = await client.PostAsJsonAsync(url, request);

    //     var responseContent = await response.Content.ReadAsStringAsync();

    //     if (!response.IsSuccessStatusCode)
    //     {
    //         throw new Exception(
    //             $"URL: {url}\n" +
    //             $"Status Code: {(int)response.StatusCode} ({response.StatusCode})\n" +
    //             $"Response: {responseContent}");
    //     }

    //     if (string.IsNullOrWhiteSpace(responseContent))
    //     {
    //         return default;
    //     }

    //     return System.Text.Json.JsonSerializer.Deserialize<TResponse>(
    //         responseContent,
    //         new System.Text.Json.JsonSerializerOptions
    //         {
    //             PropertyNameCaseInsensitive = true
    //         });
    // }

    public async Task<TResponse?> GetAsync<TResponse>(
        string url)
    {
        var client = _clientFactory.CreateClient("InternalServices");

        return await client.GetFromJsonAsync<TResponse>(url);
    }
}