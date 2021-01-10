import { Component, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import {FormArray, FormControl} from "@angular/forms";

@Component({
  templateUrl: "./users.component.html",
  styleUrls: ["./users.component.css"],
})
export class UsersComponent {
  public statusMessage: 'SUCCESS' | 'ERROR'
  public userRole
  private users: User[];
  private readonly httpClient: HttpClient;
  private readonly baseUrl: string;

  usersRoles = new FormArray([]);

  private getUserRole() {
    this.httpClient
      .post<{role: string}>(
        this.baseUrl + "user/getRole",
        { },
        { headers: { "Content-Type": "application/x-www-form-urlencoded" } }
      )
      .subscribe(
        (result) => {
          this.userRole = result.role
          console.log(result)
        },
        (error) => {
          console.error(error);
        }
      );
  }
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
          result.map((usr) => {
            if(usr.role === 'ADMIN'){
              this.usersRoles.push(new FormControl('ADMIN'))
            } else {
              this.usersRoles.push(new FormControl('USER'))
            }
          })
        },
        (error) => console.error(error)
      );
  }

  private changeRole(id: string, role:string) {
    this.httpClient
      .post<string>(
        this.baseUrl + "user/changeRole",
        { id: id, role: role },
        { headers: { "Content-Type": "application/x-www-form-urlencoded" } }
      )
      .subscribe(
        (result) => {
          this.statusMessage = 'SUCCESS'
        },
        (error) => {
          console.error(error);
          this.statusMessage = 'ERROR'
        }
      );
  }

  constructor(http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    this.httpClient = http;
    this.baseUrl = baseUrl;
    this.fetchData();
    this.getUserRole();
  }
  updateButtonClicked(){
    this.users.forEach((user, index) => {
      if(this.usersRoles.value[index] !== user.role){
        this.changeRole(user.id, this.usersRoles.value[index])
      }
    })
    this.fetchData()
  }
}

export interface User {
  role:                 string | null;
  id:                   string;
  userName:             string;
  normalizedUserName:   string;
  email:                string;
}
