import { Component, Inject } from "@angular/core";
import { Http } from "@angular/http";

@Component({
    selector: "fetchdata",
    templateUrl: "./fetchdata.component.html"
})
export class FetchDataComponent {
    public forecasts: IWeatherForecast[];

    constructor(http: Http, @Inject("BIOMETRIC_URL") baseUrl: string) {
        http.get(baseUrl + "biometric/glucose/" + 1).subscribe(result => {
            this.forecasts = result.json() as IWeatherForecast[];
        }, error => console.error(error));
    }
}

interface IWeatherForecast {
    dateFormatted: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}
