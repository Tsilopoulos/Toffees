import { enableProdMode } from "@angular/core";
import { platformBrowserDynamic } from "@angular/platform-browser-dynamic";

import { AppModule } from "./app/app.module";
import { environment } from "./environments/environment";

export function getIdentityServerUrl() {
  return "http://localhost:5000/";
}

export function getGlucoseMicroserviceUrl() {
  return "http://localhost:5001/";
}

const providers = [
  { provide: "GLUCOSE_URL", useFactory: getGlucoseMicroserviceUrl, deps: [] },
  { provide: "IDENTITY_URL", useFactory: getIdentityServerUrl, deps: [] }
];

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic(providers).bootstrapModule(AppModule)
  .catch(err => console.log(err));
