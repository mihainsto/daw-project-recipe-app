import { Component, Inject, OnInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { ActivatedRoute, Router } from "@angular/router";
import { FormArray, FormControl, NgForm, Validators } from "@angular/forms";
import { Recipe } from "../recipes/recipes.component";

@Component({
  selector: "app-counter-component",
  templateUrl: "./createOrEditRecipe.component.html",
  styleUrls: ["./createOrEditRecipe.component.css"],
})
export class CreateOrEditRecipe implements OnInit {
  public recipe: Recipe;
  public error: boolean;
  public success: boolean;

  private readonly httpClient: HttpClient;
  private readonly baseUrl: string;
  private id: string;
  private action: "CREATE" | "UPDATE";

  name = new FormControl(null, [Validators.required]);
  kcal = new FormControl(null, [Validators.required]);
  mealType = new FormControl(null, [Validators.required]);
  difficulty = new FormControl(null, [Validators.required]);
  time = new FormControl(null, [Validators.required]);
  description = new FormControl(null, [Validators.required]);
  imageUrl = new FormControl(null);
  ingredientsQuantity = new FormArray([]);
  ingredientsName = new FormArray([]);
  ingredientsTypes = new FormArray([]);
  steps = new FormArray([]);

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
          this.name.setValue(result.name);
          this.kcal.setValue(result.kcal);
          this.time.setValue(result.time);
          this.mealType.setValue(result.mealType);
          this.difficulty.setValue(result.difficulty);
          this.description.setValue(result.description);
          this.imageUrl.setValue(result.coverPhotoUrl);
          result.ingredients.forEach((ingredient) => {
            this.ingredientsName.push(
              new FormControl(ingredient.name, [Validators.required])
            );
            this.ingredientsQuantity.push(
              new FormControl(ingredient.quantity, [Validators.required])
            );
            this.ingredientsTypes.push(
              new FormControl(ingredient.type, [Validators.required])
            );
          });
          result.steps.forEach((step) => {
            this.steps.push(new FormControl(step, [Validators.required]));
          });
        },
        (error) => {
          console.error(error);
        }
      );
  }

  private updateData(http: HttpClient, baseUrl: string) {
    http
      .post<Recipe>(
        baseUrl + "recipe/update",
        {
          id: this.id,
          name: this.name.value,
          kcal: this.kcal.value,
          time: this.time.value,
          mealType: this.mealType.value,
          difficulty: this.difficulty.value,
          description: this.description.value,
          ingredientsName: this.ingredientsName.value,
          ingredientsQuantity: this.ingredientsQuantity.value,
          ingredientsTypes: this.ingredientsTypes.value,
          steps: this.steps.value,
          coverPhotoUrl: btoa(this.imageUrl.value.toString())
            .split("=")
            .join("EQUAL"),
        },
        { headers: { "Content-Type": "application/x-www-form-urlencoded" } }
      )
      .subscribe(
        (result) => {
          console.log("Updated data");
          this.success = true;
          this.router.navigate(["recipe", this.id]);
        },
        (error) => {
          console.error(error);
          this.error = true;
        }
      );
  }

  private createRecipe(http: HttpClient, baseUrl: string) {
    http
      .post<Recipe>(
        baseUrl + "recipe/create",
        {
          id: this.id,
          name: this.name.value,
          kcal: this.kcal.value,
          time: this.time.value,
          mealType: this.mealType.value,
          difficulty: this.difficulty.value,
          description: this.description.value,
          ingredientsName: this.ingredientsName.value,
          ingredientsQuantity: this.ingredientsQuantity.value,
          ingredientsTypes: this.ingredientsTypes.value,
          steps: this.steps.value,
          coverPhotoUrl: btoa(this.imageUrl.value.toString())
            .split("=")
            .join("EQUAL"),
        },
        { headers: { "Content-Type": "application/x-www-form-urlencoded" } }
      )
      .subscribe(
        (result) => {
          this.error = false;
          this.success = true;
          this.router.navigate(["recipe", this.id]);
        },
        (error) => {
          console.error(error);
          this.error = true;
        }
      );
  }

  private deleteRecipe(http: HttpClient, baseUrl: string) {
    http
      .post<boolean>(
        baseUrl + "recipe/delete",
        { id: this.id },
        { headers: { "Content-Type": "application/x-www-form-urlencoded" } }
      )
      .subscribe(
        (result) => {
          this.error = false;
          this.success = true;
          this.router.navigate(["recipes"]);
        },
        (error) => {
          console.error(error);
          this.error = true;
        }
      );
  }

  constructor(
    private router: Router,
    public activatedRoute: ActivatedRoute,
    http: HttpClient,
    @Inject("BASE_URL") baseUrl: string
  ) {
    this.httpClient = http;
    this.baseUrl = baseUrl;
    this.action = "CREATE";
    this.error = false;
    this.success = false;
  }

  ngOnInit() {
    this.activatedRoute.params.subscribe((params) => {
      this.id = params["id"];
    });
    if (this.id !== "create") {
      this.fetchData(this.httpClient, this.baseUrl);
      this.action = "UPDATE";
    }
  }

  saveChangesButtonClicked() {
    if (this.action === "UPDATE")
      this.updateData(this.httpClient, this.baseUrl);

    if (this.action === "CREATE")
      this.createRecipe(this.httpClient, this.baseUrl);
  }

  deleteRecipeButtonClicked() {
    this.deleteRecipe(this.httpClient, this.baseUrl);
  }

  addNewIngredient() {
    this.ingredientsName.push(new FormControl("", [Validators.required]));
    this.ingredientsQuantity.push(new FormControl("", [Validators.required]));
    this.ingredientsTypes.push(new FormControl("", [Validators.required]));
  }

  addNewStep() {
    this.steps.push(new FormControl("", [Validators.required]));
  }

  removeIngredient(i: number) {
    this.ingredientsName.removeAt(i);
    this.ingredientsQuantity.removeAt(i);
    this.ingredientsTypes.removeAt(i);
  }

  removeStep(i: number) {
    this.steps.removeAt(i);
  }
}
