import { Component, Inject, OnInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { NgbModal, ModalDismissReasons } from "@ng-bootstrap/ng-bootstrap";

@Component({
  selector: "app-fetch-data",
  styleUrls: ["./glucose.component.css"],
  templateUrl: "./glucose.component.html"
})
export class GlucoseComponent implements OnInit {

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      reading: [null, Validators.required],
      tag: [null, Validators.required]
    });
  }

  glucoses: IGlucose[];
  closeResult: string;
  apiGatewayUrl: string;
  form: FormGroup;

  constructor(public formBuilder: FormBuilder,
    private readonly modalService: NgbModal,
    private readonly http: HttpClient, @Inject("API_GATEWAY_URL") apiGatewayUrl: string) {
    http.get<IGlucose[]>(apiGatewayUrl + `api/glucose/${localStorage.getItem("userId")}`).subscribe(result => {
      this.glucoses = result;
    }, error => console.error(error));
    this.apiGatewayUrl = apiGatewayUrl;
  }
  
  create(content) {
    this.modalService.open(content).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  }

  delete(content) {
    this.modalService.open(content).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  }

  upload() {
    const bg = new Glucose(this.form.value.reading, this.form.value.tag);
    this.http
      .post(this.apiGatewayUrl + `api/glucose/${localStorage.getItem("userId")}`, bg)
      .do((response: IGlucose) => {
        try {
          if (response) {
            
          }
        } catch (e) {
          //TODO add error handling
        }
      }, error => console.error(error));
  }

  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return "by pressing ESC";
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return "by clicking on a backdrop";
    } else {
      return `with: ${reason}`;
    }
  }
}

interface IGlucose {
  id: number;
  data: number;
  pinchDateTime: Date;
  tag: string;
}

class Glucose implements IGlucose {
  id: number;
  data: number;
  pinchDateTime: Date;
  tag: string;

  constructor(data: number, tag: string) {
    this.data = data;
    this.tag = tag;
  }
}
