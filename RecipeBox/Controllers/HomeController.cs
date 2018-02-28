
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using RecipeBox.Models;

namespace RecipeBox.Controllers
{
  public class HomeController : Controller
  {
    [HttpGet("/")]
    public ActionResult Index()
    {
        return View();
    }

    [HttpGet("/Home/Success")]
    public ActionResult Success()
    {
        return View();
    }
  }
}
