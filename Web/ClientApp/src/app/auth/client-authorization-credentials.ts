export class ClientAuthorizationCredentials {
  clientId: string;
  clientSecret: string;
  grantType: string;
  scope: string[];

  constructor(clientId: string, clientSecret: string, grantType: string, scope: string[]) {
    this.clientId = clientId;
    this.clientSecret = clientSecret;
    this.grantType = grantType;
    this.scope = scope;
  }
}
