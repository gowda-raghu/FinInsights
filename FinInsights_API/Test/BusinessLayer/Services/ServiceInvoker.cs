public class ServiceInvoker : IServiceInvoker
{
    private readonly IHttpClientFactory _clientFactory;

    public ServiceInvoker(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(
    string url,
    TRequest request)
{
    var client = _clientFactory.CreateClient("InternalServices");

    var response = await client.PostAsJsonAsync(url, request);

    var responseContent = await response.Content.ReadAsStringAsync();

    if (!response.IsSuccessStatusCode)
    {
        throw new Exception(
            $"URL: {url}\n" +
            $"Status Code: {(int)response.StatusCode} ({response.StatusCode})\n" +
            $"Response: {responseContent}");
    }

    if (string.IsNullOrWhiteSpace(responseContent))
    {
        return default;
    }

    return System.Text.Json.JsonSerializer.Deserialize<TResponse>(
        responseContent,
        new System.Text.Json.JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
}

    public async Task<TResponse?> GetAsync<TResponse>(
        string url)
    {
        var client = _clientFactory.CreateClient("InternalServices");

        return await client.GetFromJsonAsync<TResponse>(url);
    }
}