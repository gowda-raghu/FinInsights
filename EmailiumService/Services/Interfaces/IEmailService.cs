public interface IEmailiumService
{
    Task SendEmailAsync(string to, string subject, string body);
}