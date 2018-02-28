using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace RecipeBox.Models
{
    public class Recipe
    {
        private string _name;
        private string _ingredients;
        private string _instructions;
        private int _rating;
        private int _id;

        public Recipe(string name, string ingredients, string instructions, int rating, int id = 0)
        {
            _name = name;
            _ingredients = ingredients;
            _instructions = instructions;
            _rating = rating;
            _id = id;
        }

        public override int GetHashCode()
        {
             return this.GetName().GetHashCode();
        }

        public string GetName()
        {
            return _name;
        }

        public int GetId()
        {
            return _id;
        }
        public string GetIngredients()
        {
          return _ingredients;
        }
        public string GetInstructions()
        {
          return _instructions;
        }
        public int GetRating()
        {
          return _rating;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO recipes (name, ingredients, instructions, rating) VALUES (@Name, @Ingredients, @Instructions, @Rating);";

            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@Name";
            name.Value = this._name;
            cmd.Parameters.Add(name);

            MySqlParameter ingredients = new MySqlParameter();
            ingredients.ParameterName = "@Ingredients";
            ingredients.Value = this._ingredients;
            cmd.Parameters.Add(ingredients);

            MySqlParameter instructions = new MySqlParameter();
            instructions.ParameterName = "@Instructions";
            instructions.Value = this._instructions;
            cmd.Parameters.Add(instructions);

            MySqlParameter rating = new MySqlParameter();
            rating.ParameterName = "@Rating";
            rating.Value = this._rating;
            cmd.Parameters.Add(rating);



            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void AddCategory(Category newCategory)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO recipe_categories (category_id, recipe_id) VALUES (@CategoryId, @RecipeId);";

            MySqlParameter category_id = new MySqlParameter();
            category_id.ParameterName = "@CategoryId";
            category_id.Value = newCategory.GetId();
            cmd.Parameters.Add(category_id);

            MySqlParameter recipe_id = new MySqlParameter();
            recipe_id.ParameterName = "@RecipeId";
            recipe_id.Value = _id;
            cmd.Parameters.Add(recipe_id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }


        public List<Category> GetCategories()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT category_id FROM recipe_categories WHERE recipe_id = @RecipeId;";

            MySqlParameter recipeIdParameter = new MySqlParameter();
            recipeIdParameter.ParameterName = "@RecipeId";
            recipeIdParameter.Value = _id;
            cmd.Parameters.Add(recipeIdParameter);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            List<int> categoryIds = new List<int> {};
            while(rdr.Read())
            {
                int categoryId = rdr.GetInt32(0);
                categoryIds.Add(categoryId);
            }
            rdr.Dispose();

            List<Category> categories = new List<Category> {};
            foreach (int categoryId in categoryIds)
            {
                var categoryQuery = conn.CreateCommand() as MySqlCommand;
                categoryQuery.CommandText = @"SELECT * FROM categories WHERE id = @CategoryId;";

                MySqlParameter categoryIdParameter = new MySqlParameter();
                categoryIdParameter.ParameterName = "@CategoryId";
                categoryIdParameter.Value = categoryId;
                categoryQuery.Parameters.Add(categoryIdParameter);

                var categoryQueryRdr = categoryQuery.ExecuteReader() as MySqlDataReader;
                while(categoryQueryRdr.Read())
                {
                    int thisCategoryId = categoryQueryRdr.GetInt32(0);
                    string categoryName = categoryQueryRdr.GetString(1);
                    Category foundCategory = new Category(categoryName, thisCategoryId);
                    categories.Add(foundCategory);
                }
                categoryQueryRdr.Dispose();
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return categories;
        }

        public static List<Recipe> GetAll()
        {
            List<Recipe> allRecipes = new List<Recipe> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM recipes;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
              int recipeId = rdr.GetInt32(0);
              string recipeName = rdr.GetString(1);
              string recipeIngredients = rdr.GetString(2);
              string recipeInstructions = rdr.GetString(3);
              int recipeRating = rdr.GetInt32(4);
              Recipe newRecipe = new Recipe(recipeName, recipeIngredients, recipeInstructions, recipeRating, recipeId);
              allRecipes.Add(newRecipe);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allRecipes;
        }
        public static Recipe Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM recipes WHERE id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int recipeId = 0;
            string recipeName = "";
            string recipeIngredients = "";
            string recipeInstructions = "";
            int recipeRating = 0;

            while(rdr.Read())
            {
              recipeId = rdr.GetInt32(0);
              recipeName = rdr.GetString(1);
              recipeIngredients = rdr.GetString(2);
              recipeInstructions = rdr.GetString(3);
              recipeRating = rdr.GetInt32(4);
            }
            Recipe newRecipe = new Recipe(recipeName, recipeIngredients, recipeInstructions, recipeRating, recipeId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newRecipe;
        }

        public void Delete()
          {
              MySqlConnection conn = DB.Connection();
              conn.Open();
              var cmd = conn.CreateCommand() as MySqlCommand;
              cmd.CommandText = @"DELETE FROM recipes WHERE id = @searchId;";

              MySqlParameter searchId = new MySqlParameter();
              searchId.ParameterName = "@searchId";
              searchId.Value = _id;
              cmd.Parameters.Add(searchId);

              cmd.ExecuteNonQuery();
              conn.Close();
              if (conn != null)
              {
                  conn.Dispose();
              }
          }

        public void Edit(string newIngredients)
        {
          MySqlConnection conn = DB.Connection();
          conn.Open();
          var cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"UPDATE recipes SET ingredients = @newIngredients WHERE id = @searchId;";

          MySqlParameter searchId = new MySqlParameter();
          searchId.ParameterName = "@searchId";
          searchId.Value = _id;
          cmd.Parameters.Add(searchId);

          MySqlParameter ingredients = new MySqlParameter();
          ingredients.ParameterName = "@newIngredients";
          ingredients.Value = newIngredients;
          cmd.Parameters.Add(ingredients);

          cmd.ExecuteNonQuery();
          _ingredients = newIngredients;

          conn.Close();
          if (conn != null)
          {
            conn.Dispose();
          }
        }
    }
}
