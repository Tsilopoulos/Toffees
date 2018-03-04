import { Injectable, Inject } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";
import { UserAuthenticationResponse } from "./user-authentication-response";
import { Jwt } from "./jwt";
import { ClientAuthorizationCredentials } from "./client-authorization-credentials";
import "rxjs/Rx";

@Injectable()
export class AuthenticationService {

  apiGatewayUrl : string;

  constructor(private readonly httpClient: HttpClient, @Inject("API_GATEWAY_URL") apiGatewayUrl: string,
    public router: Router,
    private readonly route: ActivatedRoute
  ) { this.apiGatewayUrl = apiGatewayUrl }

  login(username: string, password: string) {
    return this.httpClient
      .post(this.apiGatewayUrl + "api/authentication", { username: username, password: password })
      .do((userAuthenticationResponse: UserAuthenticationResponse) => {
        try {
          if (userAuthenticationResponse) {
            localStorage.setItem("userId", userAuthenticationResponse.userId);
          }
        } catch (e) {
          //TODO add error handling
        }
    }, error => console.error(error));
  }

  getJwt() {
    const clientCredentials = new ClientAuthorizationCredentials("angular",
      "93A905DE-7760-4E00-AFC9-B421820F6B70",
      "client_credentials",
      ["biometric_api.full_access"]);
    return this.httpClient
      .post(this.apiGatewayUrl + "api/authorization", clientCredentials)
      .do((jwt: Jwt) => {
        localStorage.setItem("jwt", jwt.access_token);
      }, error => console.error(error));
  }

  //register(data: any) {
  //  const postData: any = {};
  //  postData.email = data.email;
  //  postData.password = data.password;
  //  postData.password_repeat = data.password_repeat;
  //  postData.first_name = data.first_name;
  //  postData.last_name = data.last_name;

  //  return this.httpClient.post("/api/auth/register", postData);
  //}
}
