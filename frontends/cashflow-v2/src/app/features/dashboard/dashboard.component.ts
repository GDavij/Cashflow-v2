import { Component } from '@angular/core';
import { ButtonComponent } from '../../components/button/button.component';
import { RouterLink } from '@angular/router';
import { DividerComponent } from "../../components/divider/divider.component";

@Component({
  selector: 'app-dashboard',
  imports: [ButtonComponent, RouterLink, DividerComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {
  readonly features: Feature[] = [
    {
      name: 'Financial Boundaries',
      description: 'Manage your financial boundaries',
      route: '/categories',
      callToAction: 'Budget Smarter'
    },
    {
      name: 'Financial Distribution',
      description: 'Distribute your funds',
      route: '/bank-accounts',
      callToAction: ' Track your Money Distribution'
    }
];
}

type Feature = {
  name: string;
  description: string;
  route: string;
  callToAction: string;
}