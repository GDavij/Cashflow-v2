import { Component, Input, ViewChild } from '@angular/core';
import { ChartConfiguration, ChartData } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';
import { DateHelper } from '../../helpers/date.helper';

@Component({
  selector: 'app-bar-chart',
  imports: [BaseChartDirective],
  templateUrl: './bar-chart.component.html',
  styleUrl: './bar-chart.component.scss'
})
export class BarChartComponent {
  readonly barChartType = 'bar' as const;
  readonly barChartOptions: ChartConfiguration<'bar'>['options'] = {
    // We use these empty structures as placeholders for dynamic theming.
    scales: {
      x: {},
      y: {
        min: 0,
      },
    },
    plugins: {
      legend: {
        display: true,
      },
    },
  };

  @Input({ required: true }) currentDate!: DateHelper
  @Input({ required: true }) dataSource!: any;
  @Input({ required: true }) transformAction!: (dateHelper: DateHelper, dataSource: any) => ChartData<'bar'>

  @ViewChild(BaseChartDirective)
  barChartViewRef: BaseChartDirective<'bar'> | undefined;
}
