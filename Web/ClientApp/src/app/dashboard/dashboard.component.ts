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
  foo = [
    {
      "name": "Germany",
      "series": [
        {
          "name": "2010",
          "x": 40632,
          "y": 80.3,
          "r": 80.4
        },
        {
          "name": "2000",
          "x": 36953,
          "y": 80.3,
          "r": 78
        },
        {
          "name": "1990",
          "x": 31476,
          "y": 75.4,
          "r": 79
        }
      ]
    },
    {
      "name": "USA",
      "series": [
        {
          "name": "2010",
          "x": 49737,
          "y": 78.8,
          "r": 310
        },
        {
          "name": "2000",
          "x": 45986,
          "y": 76.9,
          "r": 283
        },
        {
          "name": "1990",
          "x": 3706,
          "y": 75.4,
          "r": 253
        }
      ]
    },
    {
      "name": "France",
      "series": [
        {
          "name": "2010",
          "x": 36745,
          "y": 81.4,
          "r": 63
        },
        {
          "name": "2000",
          "x": 34774,
          "y": 79.1,
          "r": 59.4
        },
        {
          "name": "1990",
          "x": 29476,
          "y": 77.2,
          "r": 56.9
        }
      ]
    },
    {
      "name": "United Kingdom",
      "series": [
        {
          "name": "2010",
          "x": 36240,
          "y": 80.2,
          "r": 62.7
        },
        {
          "name": "2000",
          "x": 32543,
          "y": 77.8,
          "r": 58.9
        },
        {
          "name": "1990",
          "x": 26424,
          "y": 75.7,
          "r": 57.1
        }
      ]
    }
  ];

  constructor(private readonly glucoseService: GlucoseService,
    private readonly http: HttpClient,
    @Inject("API_GATEWAY_URL") apiGatewayUrl: string) {
      this.dataLoaded = false;
      http.get<IGlucose[]>(apiGatewayUrl + `api/glucose/${localStorage.getItem("userId")}`).subscribe(result => {
        const series = [{ "name": "BG", "series": this.transformToD3DataSeries(result) }];
        const multi = this.separateReferenceDataSeries(result);
        const boo = this.foo;
        Object.assign(this, { series, multi, boo });
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
