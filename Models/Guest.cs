#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WeddingPlanner.Models;

public class Guest
{
    [Key]
    public int GuestId { get; set; }

    [Required(ErrorMessage = "is required")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "is required")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "is required")]
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "is required")]
    [MinLength(8, ErrorMessage = "must be at least 8 chars")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [NotMapped]
    [Compare("Password", ErrorMessage = "doesn't match password")]
    [DataType(DataType.Password)]
    [Display(Name = "PW Confirm")]
    public string ConfirmPassword { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public List<Association> Weddings { get; set; } = new List<Association>();
}