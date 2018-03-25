export class Jwt {
  access_token: string;
  expires_in: number;
  token_type: string;

  constructor(accessToken: string, expiresIn: number, tokenType: string) {
    this.access_token = accessToken;
    this.expires_in = expiresIn;
    this.token_type = tokenType;
  }
}
