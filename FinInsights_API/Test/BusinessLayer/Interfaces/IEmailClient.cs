public interface IEmailClient
{
     Task<bool> SendMailAsync(EmailRequestDto emailRequest);
}
