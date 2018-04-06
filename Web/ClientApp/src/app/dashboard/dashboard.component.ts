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
  single: any[];
  multi: any[];
  view: any[] = [700, 350];
  curve = d3.curveMonotoneX;
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
        const series = this.separateReferenceDataSeries(result);
        const single = [{ "name": "BG", "series": this.transformToD3SingleSeries(result) }];
        const multi = this.transformToD3MultiSeries(result);
        Object.assign(this, { series, single, multi });
        this.dataLoaded = true;
      }, error => console.error(error));
  }

  onSelect(event) {
    console.log(event);
  }

  private transformToD3SingleSeries(glucoses: IGlucose[]): Object[] {
    const series = [];
    glucoses.forEach(function (glucose) {
      const bgDataPoint = { "name": new Date(glucose.pinchDateTime), "value": glucose.data };
      series.push(bgDataPoint);
    });
    return series;
  }

  private transformToD3MultiSeries(glucoses: IGlucose[]): Object[] {
    const lowSeries = [];
    const normalSeries = [];
    const highSeries = [];
    glucoses.forEach(function (glucose) {
      if (glucose.data < 80) {
        lowSeries.push({
          "name": glucose.tag,
          "x": glucose.pinchDateTime,
          "y": glucose.data,
          "r": 62.7
        });
      } else if (glucose.data < 180) {
        normalSeries.push({
          "name": glucose.tag,
          "x": glucose.pinchDateTime,
          "y": glucose.data,
          "r": 62.7
        });
      } else {
        highSeries.push({
          "name": glucose.tag,
          "x": glucose.pinchDateTime,
          "y": glucose.data,
          "r": 62.7
        });
      }
    });
    return [
      { "name": "Normal", "series": normalSeries },
      { "name": "Low", "series": lowSeries },
      { "name": "High", "series": highSeries }
    ];
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
