import { Component, ViewChild } from '@angular/core';
import { ChartConfiguration, ChartData, ChartType } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';

@Component({
  standalone: true,
  imports: [BaseChartDirective],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.scss']
})
export class CustomerDashboardComponent {
  @ViewChild(BaseChartDirective) chart: BaseChartDirective | undefined;

  public pieChartOptions: ChartConfiguration['options'] = {
    plugins: {
      legend: {
        display: true,
        position: 'top',
      },
    },
  };
  public pieChartData: ChartData<'pie', number[], string | string[]> = {
    labels: ['Family', 'Senior'],
    datasets: [
      {
        data: [14, 1],
      },
    ],
  };
  public pieChartType: ChartType = 'pie';

  public barChartOptions: ChartConfiguration<'bar'>['options'] = {
    // We use these empty structures as placeholders for dynamic theming.
    scales: {
      x: {},
      y: {
        min: 10,
      },
    },
    plugins: {
      legend: {
        display: true,
      },
    },
  };
  public barChartType = 'bar' as const;

  public barChartData: ChartData<'bar'> = {
    labels: ['Health Care Insurance', 'Health Plus', 'Family Floater Plan', 'Senior Citizen Plan'],
    datasets: [
      { data: [65, 59, 80, 81], label: 'Plans' },
      { data: [28, 48, 40, 19], label: 'Claims' },
    ],
  };


}



