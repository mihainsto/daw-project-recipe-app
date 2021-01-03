import { Component, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";

@Component({
  selector: "app-counter-component",
  templateUrl: "./recipes.component.html",
  styleUrls: ["./recipes.component.css"],
})
export class RecipesComponent {
  public currentCount = 0;
  public recipes: Recipe[];
  public searchValue = "";
  public filterOptions = {
    cereal: false,
    cheese: false,
    egg: false,
    oil: false,
    pasta: false,
    sauce: false,
    spice: false,
    vegetable: false,
  };
  private readonly httpClient: HttpClient;
  private readonly baseUrl: string;

  public incrementCounter() {
    this.currentCount++;
  }

  private fetchData(http: HttpClient, baseUrl: string) {
    http
      .post<Recipe[]>(
        baseUrl + "recipe/recipes",
        { query: this.searchValue, ingredientTypes: this.filterOptions },
        { headers: { "Content-Type": "application/x-www-form-urlencoded" } }
      )
      .subscribe(
        (result) => {
          this.recipes = result;
        },
        (error) => console.error(error)
      );
  }

  constructor(http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    this.httpClient = http;
    this.baseUrl = baseUrl;
    this.fetchData(http, baseUrl);
  }

  updateSearchValue(value: string) {
    this.searchValue = value;
    this.fetchData(this.httpClient, this.baseUrl);
  }

  changeFilterField(value) {
    this.filterOptions[value] = !this.filterOptions[value];
    this.fetchData(this.httpClient, this.baseUrl);
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

