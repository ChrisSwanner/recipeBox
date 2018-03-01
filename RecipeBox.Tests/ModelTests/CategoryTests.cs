using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using RecipeBox.Models;
using System;

namespace RecipeBox.Tests
{
  [TestClass]
  public class CategoryTests : IDisposable
  {
    public void Dispose()
    {
      Recipe.DeleteAll();
      Category.DeleteAll();
    }

    public CategoryTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=recipeBook_test;";
    }

    [TestMethod]
    public void Getters_IdAndNameGetters_StringAndInt()
    {
      //arrange
      Category testCategory = new Category("Fancy Foods");
      //act
      string resultName = testCategory.GetName();
      int resultId = testCategory.GetId();
      //assert
      Assert.AreEqual(resultName, "Fancy Foods");
      Assert.AreEqual(resultId, 0);
    }

    [TestMethod]
    public void GetAll_CategoriesEmptyAtFirst_0()
    {
      //arrange, act
      int result = Category.GetAll().Count;

      //assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Equals_ReturnsTrueForSameName_Category()
    {
      //arrange, act
      Category firstCategory = new Category("Fancy Foods");
      Category secondCategory = new Category("Fancy Foods");

      //assert
      Assert.AreEqual(firstCategory, secondCategory);
    }

    [TestMethod]
    public void Save_SavesCategoryToDatabase_CategoryList()
    {
      //arrange
      Category testCategory = new Category("Fancy Foods");
      testCategory.Save();

      //act
      List<Category> result = Category.GetAll();
      List<Category> testList = new List<Category>{testCategory};

      //assert
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Save_DatabaseAssignsIdToCategory_Id()
    {
      //arrange
      Category testCategory = new Category("Fancy Foods");
      testCategory.Save();

      //act
      Category savedCategory = Category.GetAll()[0];

      int result = savedCategory.GetId();
      int testId = testCategory.GetId();

      //assert
      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void Find_FindsCategoryInDatabase_Category()
    {
      //arrange
      Category testCategory = new Category("Fancy Foods");
      testCategory.Save();

      //act
      Category foundCategory = Category.Find(testCategory.GetId());

      //assert
      Assert.AreEqual(testCategory, foundCategory);
    }
    [TestMethod]
    public void Test_AddRecipe_AddsRecipeToCategory()
    {
      //Arrange
      Category testCategory = new Category("Future Foods");
      testCategory.Save();

      Recipe testRecipe = new Recipe("Popplers", "Freshly laid Omikronian young", "Find fetal Omikronian, consume", 3);;
      testRecipe.Save();

      Recipe testRecipe2 = new Recipe("Torgo's Executive Power", "BOX Delivery Network Executive", "Beat Executive to pulp, then grind into a fine powder", 5);;
      testRecipe2.Save();

      //Act
      testCategory.AddRecipe(testRecipe);
      testCategory.AddRecipe(testRecipe2);

      List<Recipe> result = testCategory.GetRecipes();
      List<Recipe> testList = new List<Recipe>{testRecipe, testRecipe2};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
  }
}
