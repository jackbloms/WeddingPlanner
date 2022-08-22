#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace WeddingPlanner.Models;

public class Wedding
{
    public int WeddingId { get; set; }

    [Required(ErrorMessage = "is required")]
    [Display(Name = "Wedder One")]
    public string WedderOne { get; set; }

    [Required(ErrorMessage = "is required")]
    [Display(Name = "Wedder Two")]
    public string WedderTwo { get; set; }

    //need validation so no past dates are allowed
    [Required(ErrorMessage = "is required")]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "is required")]
    [Display(Name = "Wedding Address")]
    public string Address { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;


    public int GuestId { get; set; }
    public Guest? Planner { get; set; } 
    public List<Association> Guests { get; set; } = new List<Association>();

    public string WeddingCouple()
    {
        return WedderOne + " " + WedderTwo;
    }
}