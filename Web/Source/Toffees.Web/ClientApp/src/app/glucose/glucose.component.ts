import { Component, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";

@Component({
  selector: "app-fetch-data",
  templateUrl: "./glucose.component.html"
})
export class GlucoseComponent {
  public glucoses: IGlucose[];

  constructor(http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    http.get<IGlucose[]>(baseUrl + "api/biometric/glucose").subscribe(result => {
      this.glucoses = result;
    }, error => console.error(error));
  }
}

interface IGlucose {
  data: number;
  pinchDateTime: Date;
  tag: string;
}
