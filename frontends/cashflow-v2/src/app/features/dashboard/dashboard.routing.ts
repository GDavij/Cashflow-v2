import { Routes } from "@angular/router";
import { DashboardComponent } from "./dashboard.component";
import { financialBoundariesRoutes } from "./financial-boundaries/financial-boundaries.routing";
import { financialDistributionRoutes } from "./financial-distribution/financial-distribution.routing";

export const dashboardRoutes: Routes = [
    {
        path: '',
        component: DashboardComponent
    },
    ...financialBoundariesRoutes,
    ...financialDistributionRoutes
]