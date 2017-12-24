//import { Injectable } from "@angular/core";
//import { ActivatedRoute, Router } from "@angular/router";
//import { Http } from "@angular/http";
//import "rxjs/Rx";

//@Injectable()
//export class AuthenticationService {

//    returnUrl: string;

//    constructor(public http: Http,
//        public router: Router,
//        private route: ActivatedRoute
//    ) {
//    }

//    register(data: any) {
//        const postData: any = {};
//        postData.email = data.email;
//        postData.password = data.password;
//        postData.password_repeat = data.password_repeat;
//        postData.first_name = data.first_name;
//        postData.last_name = data.last_name;

//        return this.http.post("/api/auth/register", postData);
//    }

//    public getToken(): string {
//        return (localStorage.getItem("token")) as any;
//    }

//    //public isAuthenticated(): boolean {
//    //    // get the token
//    //    const token = this.getToken();
//    //    // return a boolean reflecting 
//    //    // whether or not the token is expired
//    //    return tokenNotExpired(null, token);
//    //}

//    refreshToken(data: string) {
//        return this.http.post("/api/auth/refresh", { _refreshToken: 1 });
//    }

//    login(email: string, password: string) {
//        return this.http.post("/api/auth/login", { email: email, password: password })
//            .do((data: any) => {

//                try {
//                    if (data.data && data.data.token) {
//                        //this.siteService.setCurrentUser(data.data.user);
//                        //this.siteService.setCurrentUserPermissions(data.data.permissions);
//                        //this.siteService.setJWToken(data.data.token);
//                        //this.siteService.setRefreshJWToken(data.data.refreshToken);
//                        //this.siteService.connectIo(data.data.refreshToken);

//                        this.returnUrl = this.route.snapshot.queryParams["returnUrl"] || "/";
//                        if (this.returnUrl === "/") {
//                            this.router.navigateByUrl("/admin");
//                        } else {
//                            this.router.navigate([this.returnUrl]);
//                        }
//                    }
//                } catch (e) {

//                }
//                return data.data;
//            });
//    }

//    logout() {
//        return this.http.post("/api/auth/logout", { token: this.getToken() })
//            .do((response: any) => {
//                //this.siteService.removeCurrentUser();
//                //this.siteService.removeCurrentUserPermissions();
//                //this.siteService.removeJWToken();
//                //this.siteService.removeRefreshJWToken();
//                this.router.navigateByUrl("/auth/login");
//                return response;
//            });
//    }
//}

import { Response, RequestOptions, ConnectionBackend } from "@angular/http";
import { Observable } from "rxjs/Observable";
import { HttpAuthInterceptor, InterceptorConfig } from "../interceptors/auth.interceptor";

export class HttpAuth extends HttpAuthInterceptor {

    // In production code do not put your API keys here make sure they are obtained some other way.
    // perhaps a env variables.
    private API_ACCESS_KEY = "...";
    private API_ACCESS_SECRET = "...";

    constructor(backend: ConnectionBackend, defaultOptions: RequestOptions) {
        super(backend, defaultOptions, new InterceptorConfig({ noTokenError: true }));
    }

    protected getToken(): string {
        return localStorage.getItem("id_token") as string;
    }

    protected saveToken(token: string): void {
        return localStorage.setItem("id_token", token);
    }

    protected refreshToken(): Observable<Response> {
        return super.post("http://localhost:5000/connect/token", {
            access_key_id: this.API_ACCESS_KEY,
            access_key_secret: this.API_ACCESS_SECRET
        });
    }
}
