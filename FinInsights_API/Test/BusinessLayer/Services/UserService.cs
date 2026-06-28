using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Test.DataLayer;
using Test.DataModels;

public class UserService : IUserService
{
    private readonly RaghuDbContext _context;
    private readonly IEmailClient _emailClient;

    public UserService(RaghuDbContext context, IEmailClient emailClient)
    {
        _context = context;
        _emailClient = emailClient;
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await _context.Users!.ToListAsync();
    }

    public async Task<List<User>> GetUserById(Guid id)
    {
        return await _context.Users!
        .Where(x => x.Id == id)
        .ToListAsync();
    }
    
    public async Task<User?> GetUserByID(Guid id)
    {
        return await _context.Users!
        .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _context.Users!.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<UserDto> CreateUser(UserDto dto)
    {
        try
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var user = new User
            {
                Username = dto.Username,
                Password = hashedPassword,
                Email = dto.Email
            };
            var pendingUser = await _context.PendingRegistration!.FirstOrDefaultAsync(x => x.Email == dto.Email);
            if (pendingUser?.Otp == dto.Otp)
            {
                _context.Users!.Add(user);
                if (pendingUser != null)
                {
                    pendingUser.IsUsed = true;
                }
                await _context.SaveChangesAsync();
                dto.IsRegistered = true;
                return dto;
            }
            dto.IsRegistered = false;
            dto.Message = "Invalid OTP.";
            return dto;

        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<UserDto> GenerateOtp(UserDto dto)
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        var user = new User
        {
            Username = dto.Username,
            Password = hashedPassword,
            Email = dto.Email
        };
        var pendingUser = await _context.PendingRegistration!.FirstOrDefaultAsync(x => x.Email == dto.Email);
        if (pendingUser==null || (pendingUser != null && pendingUser.IsUsed == false))
        {
            var otp = Random.Shared.Next(100000, 999999).ToString();
            var pendingregistration = new PendingRegistration
            {

                Username = user.Username,

                Email = user.Email,

                PasswordHash = user.Password,

                Otp = otp,

                ExpiryTime = DateTime.UtcNow.AddMinutes(5),

                IsUsed = false
            };
            var otpMail = new EmailRequestDto
            {
                To = user.Email,
                Subject = "Verify Your Email Address",
                Body = $@"
                        <h2>Email Verification</h2>
                        <p>Hello {user.Username},</p>
                        <p>Thank you for registering.</p>
                        <p>Your verification code is:</p>
                        <h1 style='color:#2563eb'>{otp}</h1>
                        <p>This code will expire in <strong>5 minutes</strong>.</p>
                        <p>If you did not request this registration, please ignore this email.</p>
                        <br/>
                        <p>Regards,<br/>FinInsights</p>"
            };
            var isMailsent = await _emailClient.SendMailAsync(otpMail);
            if (isMailsent)
            {
                _context.PendingRegistration!.Add(pendingregistration);
                await _context.SaveChangesAsync();
                dto.IsOtpGenerated = true;
                return dto;
            }
            dto.IsOtpGenerated = false;
            dto.Message = "Failed to send OTP";
        }
        else
        {
            dto.IsOtpGenerated=false;
            dto.Message="User Already Exists! Try again with new account.";
        }
        return dto;

    }


    public object PrettifyUser(User user)
    {
        return new
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Username,
            IsAdmin = user.IsAdmin
        };
    }

}