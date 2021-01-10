import { Component, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { AuthorizeService } from "../../api-authorization/authorize.service";

@Component({
  selector: "app-counter-component",
  templateUrl: "./recipes.component.html",
  styleUrls: ["./recipes.component.css"],
})
export class RecipesComponent {
  public currentCount = 0;
  public recipes: Recipe[];
  public favorites: number[];
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
  public onlyFav: boolean;
  public isAuthenticated: boolean;
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

  private addRecipeToFavorite(id: number) {
    this.httpClient
      .post<boolean>(
        this.baseUrl + "favorites/add",
        { id: id },
        { headers: { "Content-Type": "application/x-www-form-urlencoded" } }
      )
      .subscribe(
        (result) => {
          console.log({ result });
          this.getFavorites();
        },
        (error) => {}
      );
  }

  private removeRecipeFromFavorite(id: number) {
    this.httpClient
      .post<boolean>(
        this.baseUrl + "favorites/remove",
        { id: id },
        { headers: { "Content-Type": "application/x-www-form-urlencoded" } }
      )
      .subscribe(
        (result) => {
          // this.favorites.splice(this.favorites.indexOf(id), 1);
          this.getFavorites();
        },
        (error) => {}
      );
  }

  private getFavorites() {
    this.httpClient
      .post<number[]>(
        this.baseUrl + "favorites/get",
        { id: "test" },
        { headers: { "Content-Type": "application/x-www-form-urlencoded" } }
      )
      .subscribe(
        (result) => {
          this.favorites = result;
          console.log(this.favorites.indexOf(18));
        },
        (error) => {}
      );
  }

  constructor(
    private authorizeService: AuthorizeService,
    http: HttpClient,
    @Inject("BASE_URL") baseUrl: string
  ) {
    this.httpClient = http;
    this.baseUrl = baseUrl;
    this.fetchData(http, baseUrl);

    this.onlyFav = false;
  }

  ngOnInit() {
    this.authorizeService.isAuthenticated().subscribe((value) => {
      this.isAuthenticated = value;
      if(value){
        this.getFavorites();
      }
    });
  }
  updateSearchValue(value: string) {
    this.searchValue = value;
    this.fetchData(this.httpClient, this.baseUrl);
  }

  changeFilterField(value) {
    this.filterOptions[value] = !this.filterOptions[value];
    this.fetchData(this.httpClient, this.baseUrl);
  }

  onlyFavClicked(b: boolean) {
    this.onlyFav = b;
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
  coverPhotoUrl?: string;
}

export interface Ingredient {
  id: number;
  name: string;
  quantity: string;
  type: string;
  recipeId: number;
}
