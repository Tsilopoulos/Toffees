import { Injectable } from "@angular/core";
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest } from "@angular/common/http";
import { Observable } from "rxjs/Rx";
import "rxjs/add/observable/throw"
import "rxjs/add/operator/catch";

@Injectable()
export class AuthHttpInterceptor implements HttpInterceptor {

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const authReq = req.clone({ headers: req.headers.set("Authorization", `Bearer ${localStorage.getItem("jwt")}`) });
    return next.handle(authReq)
      .catch((error) => {
        return Observable.throw(error);
      }) as any;
  }
}
