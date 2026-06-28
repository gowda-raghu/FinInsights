using SendGrid;
using SendGrid.Helpers.Mail;

public class EmailiumService : IEmailiumService
{
    private readonly IConfiguration _config;

    public EmailiumService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var apiKey = _config["SendGrid:ApiKey"];
        var client = new SendGridClient(apiKey);

        var from = new EmailAddress(_config["SendGrid:FromEmail"], "Fin-Insights");

        var msg = MailHelper.CreateSingleEmail(
            from,
            new EmailAddress(to),
            subject,
            body,
            body
        );

        await client.SendEmailAsync(msg);
    }
}