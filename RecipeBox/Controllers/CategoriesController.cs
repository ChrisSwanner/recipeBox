using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using RecipeBox.Models;

namespace RecipeBox.Controllers
{
  public class CategoriesController : Controller
  {
    [HttpGet("/categories")]
    public ActionResult Index()
    {
      List<Category> allCategories = Category.GetAll();
      return View(allCategories);
    }

    [HttpGet("/categories/new")]
    public ActionResult CreateForm()
    {
      return View();
    }

    [HttpPost("/categories")]
    public ActionResult Create()
    {
      Category newCategory = new Category(Request.Form["category-name"]);
      newCategory.Save();
      return RedirectToAction("Success", "Home");
    }

    [HttpGet("/categories/{id}")]
    public ActionResult Details(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Category selectedCategory = Category.Find(id);
      List<Recipe> categoryRecipes = selectedCategory.GetRecipes();
      List<Recipe> allRecipes = Recipe.GetAll();
      model.Add("selectedCategory", selectedCategory);
      model.Add("categoryRecipes", categoryRecipes);
      model.Add("allRecipes", allRecipes);
      return View(model);
    }

    [HttpPost("/categories/{categoryId}/recipes/new")]
    public ActionResult AddRecipe(int categoryId)
    {
      Category category = Category.Find(categoryId);
      Recipe recipe = Recipe.Find(Int32.Parse(Request.Form["recipe-id"]));
      category.AddRecipe(recipe);
      return RedirectToAction("Details",  new { id = categoryId });
    }

    [HttpGet("/categories/{id}/delete")]
    public ActionResult DeleteCategory(int id)
    {
    Category thisCategory = Category.Find(id);
    thisCategory.Delete();
    return RedirectToAction("Index");
    }
  }
}
