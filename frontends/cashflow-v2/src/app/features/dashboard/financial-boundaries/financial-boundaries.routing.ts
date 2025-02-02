import { Routes } from "@angular/router";
import { CategoriesComponent } from "./categories/categories.component";
import { EditComponent } from "./categories/edit/edit.component";

export const financialBoundariesRoutes: Routes = [
  {
    path: 'categories',
    component: CategoriesComponent
  },
  {
    path: 'categories/create',
    component: EditComponent
  },
  {
    path: 'categories/edit/:id',
    component: EditComponent
  }
];
