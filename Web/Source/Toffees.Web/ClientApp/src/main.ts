import { enableProdMode } from "@angular/core";
import { platformBrowserDynamic } from "@angular/platform-browser-dynamic";

import { AppModule } from "./app/app.module";
import { environment } from "./environments/environment";

export function getApiGatewayUrl() {
  return "http://localhost:52633/";
}

const providers = [
  { provide: "API_GATEWAY_URL", useFactory: getApiGatewayUrl, deps: [] }
];

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic(providers).bootstrapModule(AppModule)
  .catch(err => console.log(err));
