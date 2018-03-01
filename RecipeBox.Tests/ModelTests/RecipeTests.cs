using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using RecipeBox.Models;
using System;

namespace RecipeBox.Tests
{
  [TestClass]
  public class RecipeTests : IDisposable
  {
    public void Dispose()
    {
      Recipe.DeleteAll();
      Category.DeleteAll();
    }

    public RecipeTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=recipeBook_test;";
    }

    [TestMethod]
    public void GetRecipeDetails_FetchARecipesDetails_Recipe()
    {
      //arrange
      Recipe testRecipe = new Recipe("Popplers", "Freshly laid Omikronian young", "Find fetal Omikronian, consume", 3);

      //act
      string testName = testRecipe.GetName();
      string testIngredients = testRecipe.GetIngredients();
      string testInstructions = testRecipe.GetInstructions();
      int testRating = testRecipe.GetRating();

      //assert
      Assert.AreEqual(testName,"Popplers");
      Assert.AreEqual(testIngredients,"Freshly laid Omikronian young");
      Assert.AreEqual(testInstructions,"Find fetal Omikronian, consume");
      Assert.AreEqual(testRating, 3);
    }

    [TestMethod]
    public void GetAll_DatabaseEmptyAtFirst_0()
    {
      //arrange, act
      int result = Recipe.GetAll().Count;

      //assert
      Assert.AreEqual(0, result);
    }


    public void Save_AssignsIdToObject_Id()
    {
      //arrange
      Recipe testRecipe = new Recipe("Popplers", "Freshly laid Omikronian young", "Find fetal Omikronian, consume", 3);

      //act
      testRecipe.Save();
      Recipe savedRecipe = Recipe.GetAll()[0];

      int result = savedRecipe.GetId();
      int testId = testRecipe.GetId();

      //assert
      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void Find_FindsRecipeInDatabase_Recipe()
    {
      //Arrange
      Recipe testRecipe = new Recipe("Popplers", "Freshly laid Omikronian young", "Find fetal Omikronian, consume", 3);
      testRecipe.Save();

      //act
      Recipe foundRecipe = Recipe.Find(testRecipe.GetId());

      //assert
      Assert.AreEqual(testRecipe, foundRecipe);
    }

    [TestMethod]
    public void Equals_ReturnsTrueIfNamesAreTheSame_Recipe()
    {
      //Arrange, act
      Recipe firstRecipe = new Recipe("Popplers", "Freshly laid Omikronian young", "Find fetal Omikronian, consume", 3);
      Recipe secondRecipe = new Recipe("Popplers", "Freshly laid Omikronian young", "Find fetal Omikronian, consume", 3);

      //Assert
      Assert.AreEqual(firstRecipe, secondRecipe);
    }
  }
}
