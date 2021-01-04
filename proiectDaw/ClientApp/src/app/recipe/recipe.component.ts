import { Component, Inject, OnInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { ActivatedRoute } from "@angular/router";
import { map } from "rxjs/operators";
import { Ingredient, Recipe } from "../recipes/recipes.component";
import { FormControl } from "@angular/forms";
import { AuthorizeService } from "../../api-authorization/authorize.service";

@Component({
  selector: "app-counter-component",
  templateUrl: "./recipe.component.html",
  styleUrls: ["./recipe.component.css"],
})
export class RecipeComponent implements OnInit {
  public recipe: Recipe;
  public reviews: Review[];
  public reviewButtonState: "Create" | "Update";
  public errorMessage: string | null;
  private readonly httpClient: HttpClient;
  private readonly baseUrl: string;
  private id: string;
  private currentReviewInUpdateId: number | null;

  public isAuthenticated: boolean;

  review = new FormControl(null);
  public userName: string;

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
          if (result) {
            this.reviews = result;
          }
        },
        (error) => console.error(error)
      );
  }

  private AddReview(http: HttpClient, baseUrl: string) {
    http
      .post<number>(
        baseUrl + "review/create",
        { recipeId: this.id, reviewText: this.review.value },
        { headers: { "Content-Type": "application/x-www-form-urlencoded" } }
      )
      .subscribe(
        (result) => {
          if (result) {
            this.reviews.push({
              id: result,
              text: this.review.value,
              userEmail: this.userName,
            });
            this.review = new FormControl(null);
          }
        },
        (error) => {
          console.error(error);
          this.errorMessage = "Error occured";
        }
      );
  }

  private DeleteReview(http: HttpClient, baseUrl: string, reviewId: number) {
    http
      .post<number>(
        baseUrl + "review/delete",
        { id: reviewId },
        { headers: { "Content-Type": "application/x-www-form-urlencoded" } }
      )
      .subscribe(
        (result) => {
          if (result) {
            this.reviews = this.reviews.filter((el)=> el.id !== reviewId)
            this.review = new FormControl(null);
          } else {
            this.errorMessage = "Error occured";
          }
        },
        (error) => {
          console.error(error);
          this.errorMessage = "Error occured";
        }
      );
  }

  private UpdateReview(http: HttpClient, baseUrl: string, reviewId: number) {
    http
      .post<number>(
        baseUrl + "review/update",
        { id: reviewId, reviewText: this.review.value },
        { headers: { "Content-Type": "application/x-www-form-urlencoded" } }
      )
      .subscribe(
        (result) => {
          if (result) {
            this.reviews = this.reviews.map((el) => {
              return el.id === reviewId
                ? {
                    ...el,
                    text: this.review.value,
                  }
                : el;
            });
            this.review = new FormControl(null);
            this.reviewButtonState = "Create";
          } else {
            this.errorMessage = "Error occured";
          }
        },
        (error) => {
          console.error(error);
          this.errorMessage = "Error occured";
        }
      );
  }

  constructor(
    private authorizeService: AuthorizeService,
    public activatedRoute: ActivatedRoute,
    http: HttpClient,
    @Inject("BASE_URL") baseUrl: string
  ) {
    this.httpClient = http;
    this.baseUrl = baseUrl;
    this.reviewButtonState = "Create";
    this.currentReviewInUpdateId = null;
  }

  ngOnInit() {
    this.activatedRoute.params.subscribe((params) => {
      this.id = params["id"];
    });
    this.fetchData(this.httpClient, this.baseUrl);
    this.fetchReviews(this.httpClient, this.baseUrl);
    this.authorizeService.isAuthenticated().subscribe((value) => {
      this.isAuthenticated = value;
    });
    this.authorizeService
      .getUser()
      .pipe(map((u) => u && u.name))
      .subscribe((value) => {
        this.userName = value;
      });
  }

  addReviewButtonClicked() {
    if (!this.review.value || this.review.value === "") return;

    this.AddReview(this.httpClient, this.baseUrl);
  }

  editReviewClicked(review: Review) {
    this.reviewButtonState = "Update";
    this.review = new FormControl(review.text);
    this.currentReviewInUpdateId = review.id;
  }

  deleteReviewClicked(review: Review) {
    this.DeleteReview(this.httpClient, this.baseUrl, review.id);
  }

  cancelUpdateReview() {
    this.reviewButtonState = "Create";
    this.review = new FormControl(null);
    this.currentReviewInUpdateId = null;
  }

  updateReviewClicked() {
    this.UpdateReview(
      this.httpClient,
      this.baseUrl,
      this.currentReviewInUpdateId
    );
  }
}

export interface Review {
  id: number;
  text: string;
  userEmail: string;
}
