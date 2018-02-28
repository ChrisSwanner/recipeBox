using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using RecipeBox.Models;

namespace RecipeBox.Controllers
{
  public class RecipesController : Controller
  {
    [HttpGet("/recipes")]
    public ActionResult Index()
    {
      List<Recipe> allRecipes = Recipe.GetAll();
      return View(allRecipes);
    }

    [HttpGet("/recipes/new")]
    public ActionResult CreateForm()
    {
      return View();
    }
    [HttpPost("/recipes")]
    public ActionResult Create()
    {
      Recipe newRecipe = new Recipe(Request.Form["recipe-name"], Request.Form["recipe-ingredients"], Request.Form["recipe-instructions"], (Int32.Parse(Request.Form["recipe-rating"])));
      newRecipe.Save();
      return RedirectToAction("Success", "Home");
    }

    [HttpGet("/recipes/{id}")]
    public ActionResult Details(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Recipe selectedRecipe = Recipe.Find(id);
      List<Category> recipeCategories = selectedRecipe.GetCategories();
      List<Category> allCategories = Category.GetAll();
      model.Add("selectedRecipe", selectedRecipe);
      model.Add("recipeCategories", recipeCategories);
      model.Add("allCategories", allCategories);
      return View(model);
    }

    [HttpPost("/recipes/{recipeId}/categories/new")]
     public ActionResult AddCategory(int recipeId)
     {
       Recipe recipe = Recipe.Find(recipeId);
       Category category = Category.Find(Int32.Parse(Request.Form["category-id"]));
       recipe.AddCategory(category);
       return RedirectToAction("Details",  new { id = recipeId });
     }

     [HttpGet ("/recipes/{id}/delete")]
     public ActionResult DeleteRecipe(int id)
     {
       Recipe thisRecipe = Recipe.Find(id);
       thisRecipe.Delete();
       return View("Success");
     }

     [HttpGet("/recipes/{id}/update")]
     public ActionResult UpdateForm(int id)
     {
       Recipe thisRecipe = Recipe.Find(id);
       return View(thisRecipe);
     }

     [HttpPost("/recipes/{id}/update")]
     public ActionResult UpdateItem(int id)
     {
       Recipe thisRecipe = Recipe.Find(id);
       thisRecipe.Edit(Request.Form["new-ingredient"]);
       return View("Details", thisRecipe);
     }
  }
}