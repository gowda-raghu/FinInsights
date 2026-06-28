using System.Net.Http.Headers;

public class ServiceTokenHandler : DelegatingHandler
{
    private readonly JwtService _jwtService;

    public ServiceTokenHandler(JwtService jwtService)
    {
        _jwtService = jwtService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = _jwtService.GenerateServiceToken();
        Console.WriteLine("Generated Service Token:");
        Console.WriteLine(token);
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, cancellationToken);
    }
}