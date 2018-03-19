import { Component, OnInit, Inject } from "@angular/core";
import { NgIf } from "@angular/common";
import { HttpClient } from "@angular/common/http";
import { GlucoseService } from "../glucose/glucose.service";
import { IGlucose } from "../glucose/glucose.component";
import * as d3 from "d3-shape";

@Component({
  selector: "app-dashboard",
  styleUrls: ["./dashboard.component.css"],
  templateUrl: "./dashboard.component.html"
})
export class DashboardComponent {

  dataLoaded: boolean;
  series: any[];
  multi: any[];
  view: any[] = [700, 400];
  curve = d3.curveNatural;
  showXAxis = true;
  showYAxis = true;
  gradient = false;
  showLegend = true;
  showXAxisLabel = true;
  xAxisLabel = "Time";
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
        const series = [{ "name": "BG", "series": this.transformToD3DataSeries(result) }];
        const multi = this.separateReferenceDataSeries(result);
        Object.assign(this, { series, multi });
        this.dataLoaded = true;
      }, error => console.error(error));
  }

  onSelect(event) {
    console.log(event);
  }

  private transformToD3DataSeries(glucoses: IGlucose[]): Object[] {
    const series = [];
    glucoses.forEach(function (glucose) {
      const bgDataPoint = { "name": new Date(glucose.pinchDateTime), "value": glucose.data };
      series.push(bgDataPoint);
    });
    return series;
  }

  private separateReferenceDataSeries(glucoses: IGlucose[]): Object[] {
    let lowCount = 0;
    let normalCount = 0;
    let highCount = 0;
    glucoses.forEach(function (glucose) {
      if (glucose.data < 80) {
        lowCount++;
      } else if (glucose.data < 180) {
        normalCount++;
      } else {
        highCount++;
      }
    });
    return [
      { "name": "Normal", "value": normalCount },
      { "name": "Low", "value": lowCount },
      { "name": "High", "value": highCount }
    ];
  }
}
