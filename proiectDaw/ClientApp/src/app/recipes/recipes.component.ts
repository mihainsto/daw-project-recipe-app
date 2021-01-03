import { Component, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";

@Component({
  selector: "app-counter-component",
  templateUrl: "./recipes.component.html",
})
export class RecipesComponent {
  public currentCount = 0;
  public recipes: Recipe[];

  public incrementCounter() {
    this.currentCount++;
  }

  constructor(http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    http
      .post<Recipe[]>(
        baseUrl + "recipe/recipes",
        {},
        { headers: { "Content-Type": "application/x-www-form-urlencoded" } }
      )
      .subscribe(
        (result) => {
          console.log(result);
        },
        (error) => console.error(error)
      );
  }
}

export interface Recipe {
  id: number;
  name: string;
  description: string;
  time: number;
  kcal: number;
  mealType: string;
  difficulty: string;
  steps: string[];
  ingredients: Ingredient[];
}

export interface Ingredient {
  id: number;
  name: string;
  quantity: string;
  type: string;
  recipeId: number;
}
