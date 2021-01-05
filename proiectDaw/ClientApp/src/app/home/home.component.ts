import {Component, HostListener, ViewChild} from "@angular/core";
import { interval } from "rxjs";

@Component({
  selector: "app-home",
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.css"],
})
export class HomeComponent {
  public imageUrlArray = [
    "https://images.unsplash.com/reserve/EnF7DhHROS8OMEp2pCkx_Dufer%20food%20overhead%20hig%20res.jpg?ixid=MXwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHw%3D&ixlib=rb-1.2.1&auto=format&fit=crop&w=1957&q=80",
    "https://images.unsplash.com/photo-1606265698533-c17fc6003934?ixid=MXwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHw%3D&ixlib=rb-1.2.1&auto=format&fit=crop&w=1950&q=80"
  ];
  public slideIndex: number;
  //@ts-ignore
  @ViewChild('slideshow') slideshow: any;
  public innerHeight: any;

  constructor() {
    this.slideIndex = 0;
  }
  ngOnInit() {
    this.innerHeight = window.innerHeight;
    const source = interval(6000);
    const subscribe = source.subscribe((val) => {
      if(this.slideIndex === 0){
        this.slideshow.goToSlide(1);
        this.slideIndex = 1;
      } else{
        this.slideshow.goToSlide(0);
        this.slideIndex = 0;
      }
    });
  }

  @HostListener("window:resize", ["$event"])
  onResize(event) {
    this.innerHeight = window.innerHeight;
    console.log(window.innerHeight);
  }
}
