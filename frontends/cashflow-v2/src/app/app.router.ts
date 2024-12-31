import { Routes } from "@angular/router";
import { featuresRoutes } from "./features/features.routing";
import { AppComponent } from "./app.component";
import { AdministrativeComponent } from "./layout/administrative/administrative.component";

export const appRoutes: Routes = [
  {
    path: '',
    component: AdministrativeComponent,
    children: featuresRoutes,
  }
]
