import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { HttpAuth } from "../../services/auth.service";
import { Router } from "@angular/router";

@Component({
    templateUrl: "login.component.html"
})
export class LoginComponent implements OnInit {

    submitted: boolean = false;
    form: FormGroup;

    constructor(
        public fb: FormBuilder,
        private router: Router,
        private authenticationService: HttpAuth
    ) {
    }

    ngOnInit(): void {
        this.form = this.fb.group({
            email: [null, Validators.required],
            password: [null, Validators.required]
        });
    }

    //login() {
    //    if (this.form.valid) {
    //        this.authenticationService.login(this.form.value.email, this.form.value.password).subscribe((res: any) => {
    //            if (res.error) {
    //                this.form.controls["password"].setValue("");
    //            }
    //        });
    //    } else {
    //        this.submitted = true;
    //    }
    //}
}

