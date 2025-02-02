import { Routes } from "@angular/router";
import { BankAccountsComponent } from "./bank-accounts/bank-accounts.component";
import { EditComponent } from "./bank-accounts/edit/edit.component";

export const financialDistributionRoutes: Routes = [
    {
        path: 'bank-accounts',
        component: BankAccountsComponent
    },
    {
        path: 'bank-accounts/create',
        component: EditComponent
    },
    {
        path: 'bank-accounts/edit/:id',
        component: EditComponent
    }
]