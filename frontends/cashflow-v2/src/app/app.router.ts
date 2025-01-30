import { Routes } from "@angular/router";
import { AdministrativeComponent } from "./layout/administrative/administrative.component";
import { dashboardRoutes } from "./features/dashboard/dashboard.routing";

export const appRoutes: Routes = [
  {
    path: '',
    component: AdministrativeComponent,
    children: dashboardRoutes,
  }
]
