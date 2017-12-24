import { Injectable } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";
import "rxjs/Rx";

@Injectable()
export class AuthenticationService {

    returnUrl: string;

    constructor(private httpClient: HttpClient,
        public router: Router,
        private route: ActivatedRoute
    ) {
    }

    register(data: any) {
        const postData: any = {};
        postData.email = data.email;
        postData.password = data.password;
        postData.password_repeat = data.password_repeat;
        postData.first_name = data.first_name;
        postData.last_name = data.last_name;

        return this.httpClient.post("/api/auth/register", postData);
    }

    public getToken(): string {
        return (localStorage.getItem("token")) as any;
    }

    //public isAuthenticated(): boolean {
    //    // get the token
    //    const token = this.getToken();
    //    // return a boolean reflecting 
    //    // whether or not the token is expired
    //    return tokenNotExpired(null, token);
    //}

    refreshToken(data: string) {
        return this.httpClient.post("/api/auth/refresh", { _refreshToken: 1 });
    }

    login(email: string, password: string) {
        return this.httpClient.post("/api/auth/login", { email: email, password: password })
            .do((data: any) => {

                try {
                    if (data.data && data.data.token) {
                        //this.siteService.setCurrentUser(data.data.user);
                        //this.siteService.setCurrentUserPermissions(data.data.permissions);
                        //this.siteService.setJWToken(data.data.token);
                        //this.siteService.setRefreshJWToken(data.data.refreshToken);
                        //this.siteService.connectIo(data.data.refreshToken);

                        this.returnUrl = this.route.snapshot.queryParams["returnUrl"] || "/";
                        if (this.returnUrl === "/") {
                            this.router.navigateByUrl("/admin");
                        } else {
                            this.router.navigate([this.returnUrl]);
                        }
                    }
                } catch (e) {

                }
                return data.data;
            });
    }

    //logout() {
    //    return this.httpClient.post("/api/auth/logout", { token: this.siteService.getJWToken() })
    //        .do((response: any) => {
    //            this.siteService.removeCurrentUser();
    //            this.siteService.removeCurrentUserPermissions();
    //            this.siteService.removeJWToken();
    //            this.siteService.removeRefreshJWToken();
    //            this.router.navigateByUrl("/auth/login");
    //            return response;
    //        });
    //}
}
