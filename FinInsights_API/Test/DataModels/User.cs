using System;
namespace Test.DataModels
{
    public class User
    {

        public Guid Id { get; set; }  // Primary Key

        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? Email { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public bool IsAdmin { get; set; } 


    }
}

