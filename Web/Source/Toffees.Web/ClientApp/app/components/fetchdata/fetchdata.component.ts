import { Component, Inject } from "@angular/core";
import { Http } from "@angular/http";

@Component({
    selector: "fetchdata",
    templateUrl: "./fetchdata.component.html"
})
export class FetchDataComponent {
    public glucoses: IGlucose[];

    constructor(http: Http, @Inject("BIOMETRIC_URL") baseUrl: string) {
        http.get(baseUrl + "biometric/glucose/" + 1).subscribe(result => {
            this.glucoses = result.json() as IGlucose[];
        }, error => console.error(error));
        
    }
}

interface IGlucose {
    dateFormatted: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}
