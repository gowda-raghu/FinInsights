public class PendingRegistration
{
    public Guid Id { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public string Otp { get; set; }

    public DateTime ExpiryTime { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool IsUsed { get; set; }
}