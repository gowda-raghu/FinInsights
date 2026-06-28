using System.Xml;

public class UserDto
{
    public Guid? Id { get; set; }  // Primary Key

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public bool? IsAdmin {get; set;}

    public string? Otp {get; set;}=null;

    public bool?IsOtpGenerated {get;set;}=false;

    public bool?IsRegistered {get; set;}=false;

    public string? Message{set;get;}="";

    //public DateTime CreatedDate { get; set; } = DateTime.Now;
}