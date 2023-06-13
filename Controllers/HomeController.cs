using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CRUDelicious.Models;

namespace CRUDelicious.Controllers;

public class HomeController : Controller
{
  private readonly ILogger<HomeController> _logger;
  private MyContext db;

  public HomeController(ILogger<HomeController> logger, MyContext context)
  {
    _logger = logger;
    db = context;
  }

  [HttpGet("")]
  public IActionResult Index()
  {
    List<Dish> allDishes = db.Dishes.ToList();
    return View("AllDishes", allDishes);
  }

  // redirect to New.cshtml
  [HttpGet("dishes/new")]
  public IActionResult NewDish()
  {
    return View("New");
  }

  // Create new dish
  [HttpPost("dishes/create")]
  public IActionResult CreateDish(Dish dish)
  {
    if (!ModelState.IsValid)
    {
      return View("New");
    }
    db.Dishes.Add(dish);
    db.SaveChanges();
    return RedirectToAction("Index");
  }

  // Get one Dish
  [HttpGet("dishes/{DishId}")]
  public IActionResult ShowDish(int DishId)
  {
    Dish? dish = db.Dishes.FirstOrDefault(d => d.DishId == DishId);
    if (dish == null)
    {
      return RedirectToAction("Index");
    }
    return View("ViewDish", dish);
  }

  // Redirect and auto-populate Edit Dish page
  [HttpGet("dishes/{DishId}/edit")]
  public IActionResult EditDish(int DishId)
  {
    Dish? dish = db.Dishes.FirstOrDefault(d => d.DishId == DishId);
    if (dish == null)
    {
      return RedirectToAction("Index");
    }
    return View("EditDish", dish);
  }

  // Update the Dish in the DB
  [HttpPost("dishes/{DishId}/update")]
  public IActionResult UpdateDish(Dish newDish, int DishId)
  {
    Dish? OldDish = db.Dishes.FirstOrDefault(d => d.DishId == DishId);
    if (OldDish == null)
    {
      return RedirectToAction("Index");
    }
    if (ModelState.IsValid)
    {
      OldDish.Name = newDish.Name;
      OldDish.Chef = newDish.Chef;
      OldDish.Tastiness = newDish.Tastiness;
      OldDish.Calories = newDish.Calories;
      OldDish.Description = newDish.Description;
      db.SaveChanges();
      return RedirectToAction("ShowDish", OldDish);
    }
    else
    {
      return EditDish(OldDish.DishId);
    }
  }
  // Delete the Dish
  [HttpPost("dishes/{DishId}/destroy")]
  public IActionResult DestroyDish(int DishId)
  {
    Dish? DishToDelete = db.Dishes.FirstOrDefault(d => d.DishId == DishId);
    if (DishToDelete == null)
    {
      return RedirectToAction("Index");
    }
    db.Dishes.Remove(DishToDelete);
    db.SaveChanges();
    return RedirectToAction("Index");
  }

  public IActionResult Privacy()
  {
    return View();
  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error()
  {
    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
  }
}