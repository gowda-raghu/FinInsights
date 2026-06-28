using System;
using System.ComponentModel.DataAnnotations;


public class FavouriteDto
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public int SchemeCode { get; set; }

    public string? Details { get; set; }
}
