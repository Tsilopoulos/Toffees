import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { AuthenticationService } from "../auth/authentication.service";

@Component({
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.css"],
  providers: [AuthenticationService]
})
export class LoginComponent implements OnInit {

  submitted: boolean = false;
  form: FormGroup;
  
  constructor(
    public formBuilder: FormBuilder,
    private readonly router: Router,
    private readonly authenticationService: AuthenticationService
  ) { }

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      username: [null, Validators.required],
      password: [null, Validators.required]
    });

    this.authenticationService.getJwt().subscribe((res: any) => {});
  }

  login() {
    if (this.form.valid) {
      this.authenticationService.login(this.form.value.username, this.form.value.password).subscribe((res: any) => {
        if (res.error) {
          this.form.controls["password"].setValue("");
        } else {
          this.router.navigateByUrl("/glucose");
        }        
      });
    } else {
      this.submitted = true;
    }
  }
}
