import { Component, Inject, OnInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { NgbModal, NgbModalRef, ModalDismissReasons } from "@ng-bootstrap/ng-bootstrap";
import { GlucoseService } from "./glucose.service";

@Component({
  selector: "app-fetch-data",
  styleUrls: ["./glucose.component.css"],
  templateUrl: "./glucose.component.html"
})
export class GlucoseComponent implements OnInit {

  glucoses: IGlucose[];
  closeResult: string;
  apiGatewayUrl: string;
  form: FormGroup;
  private modalRef: NgbModalRef;

  constructor(public formBuilder: FormBuilder,
    private readonly modalService: NgbModal,
    private readonly glucoseService: GlucoseService,
    private readonly http: HttpClient,
    @Inject("API_GATEWAY_URL") apiGatewayUrl: string) {
      http.get<IGlucose[]>(apiGatewayUrl + `api/glucose/${localStorage.getItem("userId")}`).subscribe(result => {
        this.glucoses = result;
      }, error => console.error(error));
      this.apiGatewayUrl = apiGatewayUrl;
  }

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      reading: [null, Validators.required],
      tag: [null, Validators.required]
    });
  }

  create(content) {
    this.modalRef = this.modalService.open(content);
    this.modalRef.result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  }

  edit() {

  }

  delete(bg: IGlucose) {
    this.modalRef = this.modalService.open(bg);
    this.modalRef.result.then((result) => {
      this.glucoseService.delete(bg.id).subscribe(() => {
        const index = this.glucoses.indexOf(bg);
        if (index !== -1) {
          this.glucoses.splice(index, 1);
        }
        this.modalRef.close();
      });
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  }

  upload() {
    const bg = new Glucose(this.form.value.reading, this.form.value.tag);
    if (this.form.valid) {
      this.glucoseService.post(bg).subscribe((newBg: IGlucose) => {
        this.glucoses.push(newBg);
        this.modalRef.close();       
      });
    } else {
      // ?!
    }
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

export interface IGlucose {
  id: number;
  data: number;
  pinchDateTime: Date;
  tag: string;
}

export class Glucose implements IGlucose {
  id: number;
  data: number;
  pinchDateTime: Date;
  tag: string;

  constructor(data: number, tag: string) {
    this.data = data;
    this.tag = tag;
  }
}
