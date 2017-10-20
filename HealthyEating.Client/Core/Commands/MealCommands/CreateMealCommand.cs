﻿using HealthyEating.Client.Core.Contracts;
using HealthyEating.Client.Data;
using HealthyEating.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthyEating.Client.Core.Factories;
using HealthyEating.Client.Utils;
using Bytes2you.Validation;
using System.ComponentModel.Design;

namespace HealthyEating.Client.Core.Commands.MealCommands
{
    class CreateMealCommand : MealCommand, ICommand
    {
        public CreateMealCommand(IModelFactory factory, IReader reader, IWriter writer, IUserManager userManager, IDatabase database)
            : base(factory, reader, writer, userManager, database)
        {

        }

        public override string Execute()
        {

            MealCategory mealCategory = (MealCategory)MealCategory.Parse(typeof(MealCategory), 
                base.ReadOneLine("Breakfast/Lunch/Supper: ").ToLower());
            DateTime mealDate = Convert.ToDateTime(base.ReadOneLine("When did you eat it? (DD/MM/YYYY): "));
            string recipeToAdd = ReadOneLine("What did you eat?: ");
            List<Recipe> listOfRecipesToAdd = new List<Recipe>();

            do
            {
                listOfRecipesToAdd.Add(this.UserManager.LoggedUser.Recipes.SingleOrDefault(r => r.Name == recipeToAdd));
                recipeToAdd = ReadOneLine("What did you eat?: ");

            }
            while (recipeToAdd != "");
            
            Meal meal = this.Factory.CreateMeal(mealCategory, mealDate, listOfRecipesToAdd);
           // this.UserManager.LoggedUser.Meals.Add(meal);
            this.Database.Meals.Add(meal);
            this.Database.SaveChanges();
            return $"Meal with Id {this.UserManager.LoggedUser.Meals.Count - 1} was created!";
        }
    }
}
