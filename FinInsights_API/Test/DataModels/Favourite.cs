using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Favourite
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public int SchemeCode { get; set; }

    public string? Details { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
