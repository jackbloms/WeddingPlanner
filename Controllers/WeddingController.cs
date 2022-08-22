using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Models;
namespace WeddingPlanner.Controllers;

public class WeddingController : Controller
{
    private MyContext db;

    public WeddingController(MyContext context)
    {
        db = context;
    }

    [HttpGet("/wedding/new")]
    public IActionResult WeddingForm()
    {
        return View("NewWedding");
    }

    [HttpGet("/wedding/detail/{wId}")]
    public IActionResult WeddingDetail(int wId)
    {
        Wedding? wed = db.Weddings
        .Include(w=>w.Guests)
        .ThenInclude(w=>w.Guest)
        .FirstOrDefault(w=>w.WeddingId == wId);

        if(wed == null)
        {
            return Redirect("Dashboard");
        }

        return View("WeddingDetail", wed);
    }

    [HttpPost("/wedding/create/")]
    public IActionResult CreateWedding(Wedding newWedding)
    {
        newWedding.GuestId = (int)HttpContext.Session.GetInt32("UUID");
        db.Weddings.Add(newWedding);
        db.SaveChanges();

        return WeddingDetail(newWedding.WeddingId);
    }

    [HttpPost("/wedding/rsvp/{wId}")]
    public IActionResult RSVP(int wId)
    {
        Association newAssociation = new Association(){
            WeddingId = wId,
            GuestId = (int)HttpContext.Session.GetInt32("UUID")
        };

        db.Associations.Add(newAssociation);
        db.SaveChanges();

        return RedirectToAction("Dashboard", "Guest");
    }

    [HttpPost("/wedding/unrsvp/{wId}")]
    public IActionResult UNRSVP(int wId)
    {
        Association? delAssociation = db.Associations
        .Include(w=>w.Wedding)
        .Include(w=>w.Guest)
        .FirstOrDefault(w=>w.WeddingId == wId);

        db.Associations.Remove(delAssociation);
        db.SaveChanges();
        
        return RedirectToAction("Dashboard", "Guest");
    }

    [HttpPost("/wedding/delete/{wId}")]
    public IActionResult Delete(int wId)
    {
        Wedding? delWed = db.Weddings.FirstOrDefault(w=>w.WeddingId == wId);

        db.Weddings.Remove(delWed);
        db.SaveChanges();
        
        return RedirectToAction("Dashboard", "Guest");
    }


}