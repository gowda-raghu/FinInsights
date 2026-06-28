using Test.DataModels;


public class EmailClient : IEmailClient
{
    private readonly IServiceInvoker _serviceInvoker;
    private readonly IConfiguration _config;
    private string url;

    public EmailClient(IServiceInvoker serviceInvoker,IConfiguration config)
    {
        _serviceInvoker=serviceInvoker;
        _config=config;
        url=_config["Services:Emailium"];
    }

    

    
    public async Task<bool> SendMailAsync(EmailRequestDto emailRequest)
    {
        var response = await _serviceInvoker.PostAsync<
            EmailRequestDto,
            bool>(
            url+"api/Email/send",
            new EmailRequestDto
            {
                To = emailRequest.To,
                Subject = emailRequest.Subject,
                Body = emailRequest.Body
            });
        return (bool)response;
    }
}