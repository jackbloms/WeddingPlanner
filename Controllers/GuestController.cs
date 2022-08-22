using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Models;
namespace WeddingPlanner.Controllers;

public class GuestController : Controller
{
    private MyContext _context;

    public GuestController(MyContext context)
    {
        _context = context;
    }

    [HttpGet("/")]
    public IActionResult LogAndReg()
    {
        return View("LogAndReg");
    }

    [HttpGet("/dashboard")]
    public IActionResult Dashboard()
    {
        List<Wedding> AllWeddings = _context.Weddings
        .Include(w=>w.Guests)
        .ThenInclude(w2=>w2.Guest)
        .ToList();
        return View("Dashboard", AllWeddings);
    }

    [HttpPost("/register")]
    public IActionResult Register(Guest newGuest)
    {   

        //add verifications later
        PasswordHasher<Guest> hashBrowns = new PasswordHasher<Guest>();
        newGuest.Password = hashBrowns.HashPassword(newGuest, newGuest.Password);

        _context.Guests.Add(newGuest);
        _context.SaveChanges();
        HttpContext.Session.SetInt32("UUID", newGuest.GuestId);

        return Dashboard();
    }

    [HttpPost("/login")]
    public IActionResult Login(LoginGuest loginGuest)
    {
        //add verifications later
        Guest? dbGuest = _context.Guests.FirstOrDefault(user => user.Email == loginGuest.LoginEmail);

        if(dbGuest == null)
        {
            ModelState.AddModelError("LoginEmail", "Not found");
            return LogAndReg();
        }

        PasswordHasher<LoginGuest> hashBrowns = new PasswordHasher<LoginGuest>();
        PasswordVerificationResult pwResult = hashBrowns.VerifyHashedPassword
        (loginGuest, dbGuest.Password, loginGuest.LoginPassword);

        if(pwResult == 0)
        {
            ModelState.AddModelError("LoginPassword", "is not correct");
            return LogAndReg();
        }

        HttpContext.Session.SetInt32("UUID", dbGuest.GuestId);
        return Dashboard();
    }

    //not working.. come back
    [HttpPost("/logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return View("LogAndReg");
    }
}