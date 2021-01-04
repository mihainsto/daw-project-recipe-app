import { Component, Inject, OnInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { ActivatedRoute } from "@angular/router";
import { map } from "rxjs/operators";
import {Ingredient, Recipe} from "../recipes/recipes.component";
import {FormControl} from "@angular/forms";

@Component({
  selector: "app-counter-component",
  templateUrl: "./recipe.component.html",
  styleUrls: ["./recipe.component.css"],
})
export class RecipeComponent implements OnInit {
  public recipe: Recipe;
  public reviews: Review[];
  private readonly httpClient: HttpClient;
  private readonly baseUrl: string;
  private id: string;

  review = new FormControl(null);

  private fetchData(http: HttpClient, baseUrl: string) {
    http
      .post<Recipe>(
        baseUrl + "recipe/recipe",
        { id: this.id },
        { headers: { "Content-Type": "application/x-www-form-urlencoded" } }
      )
      .subscribe(
        (result) => {
          this.recipe = result;
        },
        (error) => console.error(error)
      );
  }

  private fetchReviews(http: HttpClient, baseUrl: string) {
    http
      .post<Review[]>(
        baseUrl + "review/reviews",
        { id: this.id },
        { headers: { "Content-Type": "application/x-www-form-urlencoded" } }
      )
      .subscribe(
        (result) => {
          this.reviews = result;
        },
        (error) => console.error(error)
      );
  }
  constructor(
    public activatedRoute: ActivatedRoute,
    http: HttpClient,
    @Inject("BASE_URL") baseUrl: string
  ) {
    this.httpClient = http;
    this.baseUrl = baseUrl;
  }

  ngOnInit() {
    this.activatedRoute.params.subscribe((params) => {
      this.id = params["id"];
    });
    this.fetchData(this.httpClient, this.baseUrl);
    this.fetchReviews(this.httpClient, this.baseUrl)
  }
}

export interface Review {
  id: number;
  text: string;
  userEmail: string;
}
