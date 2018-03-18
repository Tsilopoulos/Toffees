import { Component, OnInit, Inject } from "@angular/core";
import { NgIf } from "@angular/common";
import { HttpClient } from "@angular/common/http";
import { GlucoseService } from "../glucose/glucose.service";
import { IGlucose } from "../glucose/glucose.component";

@Component({
  selector: "app-dashboard",
  styleUrls: ["./dashboard.component.css"],
  templateUrl: "./dashboard.component.html"
})
export class DashboardComponent {

  series: any[];
  dataLoaded: boolean;
  view: any[] = [700, 400];

  // ngx-charts options
  showXAxis = true;
  showYAxis = true;
  gradient = false;
  showLegend = true;
  showXAxisLabel = true;
  xAxisLabel = "";
  showYAxisLabel = true;
  yAxisLabel = "mg/dL";
  timeline = true;
  autoScale = false;
  showRefLines = true;
  showRefLabels = true;
  referenceLines = [
    { value: 180, name: "High" },
    { value: 80, name: "Normal" },
    { value: 40, name: "Low" }
  ];
  colorScheme = {
    domain: ["#5AA454", "#A10A28", "#C7B42C", "#AAAAAA"]
  };

  constructor(private readonly glucoseService: GlucoseService,
    private readonly http: HttpClient,
    @Inject("API_GATEWAY_URL") apiGatewayUrl: string) {
      this.dataLoaded = false;
      http.get<IGlucose[]>(apiGatewayUrl + `api/glucose/${localStorage.getItem("userId")}`).subscribe(result => {
        const series = [{ "name": "BG", "series": this.transform(result) }];
        Object.assign(this, { series });
        this.dataLoaded = true;
      }, error => console.error(error));
  }

  transform(glucoses: IGlucose[]): Object[] {
    const series = [];
    glucoses.forEach(function (glucose) {
      const bgDataPoint = { "name": glucose.pinchDateTime, "value": glucose.data };
      series.push(bgDataPoint);
    });
    return series;
  }

  onSelect(event) {
    console.log(event);
  }

}
