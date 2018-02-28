using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace RecipeBox.Models
{
    public class Category
    {
        private string _name;
        private int _id;
        public Category(string name, int id = 0)
        {
            _name = name;
            _id = id;
        }
        public override bool Equals(System.Object otherCategory)
        {
            if (!(otherCategory is Category))
            {
                return false;
            }
            else
            {
                Category newCategory = (Category) otherCategory;
                return this.GetId().Equals(newCategory.GetId());
            }
        }
        public override int GetHashCode()
        {
            return this.GetId().GetHashCode();
        }
        public string GetName()
        {
            return _name;
        }
        public int GetId()
        {
            return _id;
        }
        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO categories (name) VALUES (@name);";

            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = this._name;
            cmd.Parameters.Add(name);

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

        }
        public static List<Category> GetAll()
        {
            List<Category> allCategories = new List<Category> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM categories;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
              int CategoryId = rdr.GetInt32(0);
              string CategoryName = rdr.GetString(1);
              Category newCategory = new Category(CategoryName, CategoryId);
              allCategories.Add(newCategory);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allCategories;
        }
        public static Category Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM categories WHERE id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int CategoryId = 0;
            string CategoryName = "";

            while(rdr.Read())
            {
              CategoryId = rdr.GetInt32(0);
              CategoryName = rdr.GetString(1);
            }
            Category newCategory = new Category(CategoryName, CategoryId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newCategory;
        }

        public void AddRecipe(Recipe newRecipe)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO recipe_categories (category_id, recipe_id) VALUES (@CategoryId, @RecipeId);";

            MySqlParameter category_id = new MySqlParameter();
            category_id.ParameterName = "@CategoryId";
            category_id.Value = _id;
            cmd.Parameters.Add(category_id);

            MySqlParameter recipe_id = new MySqlParameter();
            recipe_id.ParameterName = "@RecipeId";
            recipe_id.Value = newRecipe.GetId();
            cmd.Parameters.Add(recipe_id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public void Delete()
        {
          MySqlConnection conn = DB.Connection();
          conn.Open();

          MySqlCommand cmd = new MySqlCommand("DELETE FROM categories WHERE id = @CategoryId; DELETE FROM recipe_categories WHERE category_id = @CategoryId;", conn);
          MySqlParameter categoryIdParameter = new MySqlParameter();
          categoryIdParameter.ParameterName = "@CategoryId";
          categoryIdParameter.Value = this.GetId();

          cmd.Parameters.Add(categoryIdParameter);
          cmd.ExecuteNonQuery();

          if (conn != null)
          {
            conn.Close();
          }
        }

        public List<Recipe> GetRecipes()
          {
              MySqlConnection conn = DB.Connection();
              conn.Open();
              var cmd = conn.CreateCommand() as MySqlCommand;
              cmd.CommandText = @"SELECT recipe_id FROM recipe_categories WHERE category_id = @CategoryId;";

              MySqlParameter categoryIdParameter = new MySqlParameter();
              categoryIdParameter.ParameterName = "@CategoryId";
              categoryIdParameter.Value = _id;
              cmd.Parameters.Add(categoryIdParameter);

              var rdr = cmd.ExecuteReader() as MySqlDataReader;

              List<int> recipeIds = new List<int> {};
              while(rdr.Read())
              {
                  int recipeId = rdr.GetInt32(0);
                  recipeIds.Add(recipeId);
              }
              rdr.Dispose();

              List<Recipe> recipes = new List<Recipe> {};
              foreach (int recipeId in recipeIds)
              {
                  var recipeQuery = conn.CreateCommand() as MySqlCommand;
                  recipeQuery.CommandText = @"SELECT * FROM recipes WHERE id = @RecipeId;";

                  MySqlParameter recipeIdParameter = new MySqlParameter();
                  recipeIdParameter.ParameterName = "@RecipeId";
                  recipeIdParameter.Value = recipeId;
                  recipeQuery.Parameters.Add(recipeIdParameter);

                  var recipeQueryRdr = recipeQuery.ExecuteReader() as MySqlDataReader;
                  while(recipeQueryRdr.Read())
                  {
                    int thisRecipeId = recipeQueryRdr.GetInt32(0);
                    string recipeName = recipeQueryRdr.GetString(1);
                    string recipeIngredients = recipeQueryRdr.GetString(2);
                    string recipeInstructions = recipeQueryRdr.GetString(3);
                    int recipeRating = recipeQueryRdr.GetInt32(4);
                      Recipe foundRecipe = new Recipe(recipeName, recipeIngredients, recipeInstructions, recipeRating, thisRecipeId);
                      recipes.Add(foundRecipe);
                  }
                  recipeQueryRdr.Dispose();
              }
              conn.Close();
              if (conn != null)
              {
                  conn.Dispose();
              }
              return recipes;
          }
    }
}
