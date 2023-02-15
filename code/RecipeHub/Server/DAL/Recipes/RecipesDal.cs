﻿using System.Data;
using Microsoft.Data.SqlClient;
using Server.Data.Settings;
using Shared_Resources.Model.Ingredients;
using Shared_Resources.Model.Recipes;

namespace Server.DAL.Recipes
{
    /// <summary>
    /// Contains methods for interfacing with the Recipes table
    /// </summary>
    public class RecipeDal : IRecipesDal
    {
        /// <inheritdoc/>
        public bool RecipeWithIdExists(int recipeId)
        {
            var query = "SELECT recipeId FROM Recipes WHERE recipeId = @recipeId;";

            using var connection = new SqlConnection(DatabaseSettings.ConnectionString);
            using var command = new SqlCommand(query, connection);

            command.Parameters.Add("@recipeId", SqlDbType.Int).Value = recipeId;
            connection.Open();

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public bool IsAuthorOfRecipe(int authorId, int recipeId)
        {
            var query = "SELECT authorId FROM Recipes WHERE recipeId = @recipeId AND authorId = @authorId;";

            using var connection = new SqlConnection(DatabaseSettings.ConnectionString);
            using var command = new SqlCommand(query, connection);

            command.Parameters.Add("@recipeId", SqlDbType.Int).Value = recipeId;
            command.Parameters.Add("@authorId", SqlDbType.Int).Value = authorId;
            connection.Open();

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public Recipe[] GetRecipes(int userId)
        {
            return this.GetRecipesWithName(userId, "");
        }

        /// <inheritdoc/>
        public Recipe? GetRecipe(int recipeId)
        {
            var query = "SELECT CONCAT(TRIM(Users.firstName), ' ', TRIM(Users.lastName)) AS authorName, TRIM(Recipes.name) AS name, " +
                        "TRIM(Recipes.description) AS description, Recipes.isPublic FROM Recipes, Users WHERE Recipes.recipeId = @recipeId;";
            using var connection = new SqlConnection(DatabaseSettings.ConnectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.Add("@recipeId", SqlDbType.Int).Value = recipeId;
            connection.Open();

            using var reader = command.ExecuteReader();
            var authorNameOrdinal = reader.GetOrdinal("authorName");
            var nameOrdinal = reader.GetOrdinal("name");
            var descriptionOrdinal = reader.GetOrdinal("description");
            var isPublicOrdinal = reader.GetOrdinal("isPublic");

            while (reader.Read())
            {
                var authorName = reader.GetString(authorNameOrdinal);
                var name = reader.GetString(nameOrdinal);
                var description = reader.GetString(descriptionOrdinal);
                var isPublic = reader.GetByte(isPublicOrdinal) == 1;
                return new Recipe(recipeId, authorName, name, description, isPublic);
            }

            return null;
        }

        /// <inheritdoc/>
        public Recipe[] GetRecipesWithName(int userId, string nameFilter)
        {
            var recipes = new List<Recipe>();
            var query = "SELECT Recipes.recipeId, CONCAT(TRIM(Users.firstName), ' ', TRIM(Users.lastName)) AS authorName, TRIM(Recipes.name) AS name, " +
                        "TRIM(Recipes.description) AS description, Recipes.isPublic FROM Recipes, Users WHERE Recipes.authorId = Users.userId " +
                        "AND name LIKE @nameFilter AND (Recipes.authorId = @userId OR Recipes.isPublic = 1);";

            using var connection = new SqlConnection(DatabaseSettings.ConnectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
            command.Parameters.Add("@nameFilter", SqlDbType.Char).Value = $"%{nameFilter}%";
            connection.Open();

            using var reader = command.ExecuteReader();
            var recipeIdOrdinal = reader.GetOrdinal("recipeId");
            var authorNameOrdinal = reader.GetOrdinal("authorName");
            var nameOrdinal = reader.GetOrdinal("name");
            var descriptionOrdinal = reader.GetOrdinal("description");
            var isPublicOrdinal = reader.GetOrdinal("isPublic");

            while (reader.Read())
            {
                var recipeId = reader.GetInt32(recipeIdOrdinal);
                var authorName = reader.GetString(authorNameOrdinal);
                var name = reader.GetString(nameOrdinal);
                var description = reader.GetString(descriptionOrdinal);
                var isPublic = reader.GetByte(isPublicOrdinal) == 1;
                recipes.Add(new Recipe(recipeId, authorName, name, description, isPublic));
            }

            return recipes.ToArray();
        }

        /// <inheritdoc/>
        public bool AddRecipe(int authorId, string name, string description, bool isPublic)
        {
            var query = "INSERT INTO Recipes (authorId, name, description, isPublic) VALUES (@authorId, @name, @description, @isPublic);";

            using var connection = new SqlConnection(DatabaseSettings.ConnectionString);
            using var command = new SqlCommand(query, connection);

            command.Parameters.Add("@authorId", SqlDbType.Int).Value = authorId;
            command.Parameters.Add("@name", SqlDbType.Char).Value = name;
            command.Parameters.Add("@description", SqlDbType.Char).Value = description;
            command.Parameters.Add("@isPublic", SqlDbType.TinyInt).Value = isPublic;
            connection.Open();

            var rowsAffected = command.ExecuteNonQuery();
            
            return rowsAffected == 1;
        }

        /// <inheritdoc/>
        public bool RemoveRecipe(int recipeId)
        {
            var query = "DELETE FROM Recipes WHERE recipeId = @recipeId";

            using var connection = new SqlConnection(DatabaseSettings.ConnectionString);
            using var command = new SqlCommand(query, connection);

            command.Parameters.Add("@recipeId", SqlDbType.Int).Value = recipeId;
            connection.Open();

            var rowsAffected = command.ExecuteNonQuery();

            return rowsAffected == 1;
        }

        /// <inheritdoc/>
        public bool EditRecipe(int recipeId, string name, string description, bool isPublic)
        {
            var query = "UPDATE Recipes SET name = @name, description = @description, isPublic = @isPublic " +
                        "WHERE recipeId = @recipeId;";

            using var connection = new SqlConnection(DatabaseSettings.ConnectionString);
            using var command = new SqlCommand(query, connection);

            command.Parameters.Add("@recipeId", SqlDbType.Int).Value = recipeId;
            command.Parameters.Add("@name", SqlDbType.Char).Value = name;
            command.Parameters.Add("@description", SqlDbType.Char).Value = description;
            command.Parameters.Add("@isPublic", SqlDbType.TinyInt).Value = isPublic;
            connection.Open();

            var rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        /// <inheritdoc/>
        public bool UserCanSeeRecipe(int userId, int recipeId)
        {
            var query = "SELECT authorId FROM Recipes WHERE recipeId = @recipeId AND (authorId = @userId OR isPublic = 1);";

            using var connection = new SqlConnection(DatabaseSettings.ConnectionString);
            using var command = new SqlCommand(query, connection);

            command.Parameters.Add("@recipeId", SqlDbType.Int).Value = recipeId;
            command.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
            connection.Open();

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public Shared_Resources.Model.Ingredients.Ingredient[] GetIngredientsForRecipe(int recipeId)
        {
            var ingredients = new List<Shared_Resources.Model.Ingredients.Ingredient>();
            var query = "SELECT RecipeIngredients.ingredientId, TRIM(Ingredients.name) AS name, RecipeIngredients.amount " +
                        "FROM RecipeIngredients, Ingredients WHERE RecipeIngredients.ingredientId = Ingredients.ingredientId AND recipeId = @recipeId";

            using var connection = new SqlConnection(DatabaseSettings.ConnectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.Add("@recipeId", SqlDbType.Int).Value = recipeId;
            connection.Open();

            using var reader = command.ExecuteReader();
            var ingredientIdOrdinal = reader.GetOrdinal("ingredientId");
            var nameOrdinal = reader.GetOrdinal("name");
            var amountOrdinal = reader.GetOrdinal("amount");

            while (reader.Read())
            {
                var ingredientId = reader.GetInt32(ingredientIdOrdinal);
                var name = reader.GetString(nameOrdinal);
                var amount = reader.GetInt32(amountOrdinal);
                ingredients.Add(new(name, amount, MeasurementType.Volume));
            }

            return ingredients.ToArray();
        }

        /// <inheritdoc/>
        public RecipeStep[] GetStepsForRecipe(int recipeId)
        {
            var steps = new List<RecipeStep>();
            var query = "SELECT stepNumber, TRIM(stepName) AS stepName, TRIM(instructions) AS instructions " +
                        "FROM RecipeSteps WHERE recipeId = @recipeId ORDER BY stepNumber";

            using var connection = new SqlConnection(DatabaseSettings.ConnectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.Add("@recipeId", SqlDbType.Int).Value = recipeId;
            connection.Open();

            using var reader = command.ExecuteReader();
            var stepNumberOrdinal = reader.GetOrdinal("stepNumber");
            var stepNameOrdinal = reader.GetOrdinal("stepName");
            var instructionsOrdinal = reader.GetOrdinal("instructions");

            while (reader.Read())
            {
                var stepNumber = reader.GetInt32(stepNumberOrdinal);
                var name = reader.GetString(stepNameOrdinal);
                var instructions = reader.GetString(instructionsOrdinal);
                steps.Add(new RecipeStep(stepNumber, name, instructions));
            }

            return steps.ToArray();
        }
    }
}