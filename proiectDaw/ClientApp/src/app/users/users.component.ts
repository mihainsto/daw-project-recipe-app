import { Component, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";

@Component({
  templateUrl: "./users.component.html",
  styleUrls: ["./users.component.css"],
})
export class UsersComponent {
  private users: User[];
  private readonly httpClient: HttpClient;
  private readonly baseUrl: string;

  private fetchData() {
    this.httpClient
      .post<User[]>(
        this.baseUrl + "user/get",
        {},
        { headers: { "Content-Type": "application/x-www-form-urlencoded" } }
      )
      .subscribe(
        (result) => {
          this.users = result;
        },
        (error) => console.error(error)
      );
  }

  constructor(http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    this.httpClient = http;
    this.baseUrl = baseUrl;
    this.fetchData();
  }
}

export interface User {
  role:                 string | null;
  id:                   string;
  userName:             string;
  normalizedUserName:   string;
  email:                string;
}
