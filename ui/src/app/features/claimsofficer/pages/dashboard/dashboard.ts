import { Component, OnInit, ViewChild } from '@angular/core';
import { Chart, ChartConfiguration, ChartData, ChartType } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';
import { ClaimsOfficerReportService } from '../../services/claimsofficer-report.service';

@Component({
  standalone: true,
  imports: [BaseChartDirective],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.scss']
})
export class ClaimsOfficerDashboardComponent implements OnInit {
  @ViewChild(BaseChartDirective) chart: BaseChartDirective | undefined;

  //pieChartData: ChartData<'pie', number[], string | string[]>()
  ce: any;
  constructor(private claimsOfficerReportService: ClaimsOfficerReportService) { }

  ngOnInit(): void {
    this.policiesbytypestatus();
    this.premiumvspayout();
    this.planwisepolicyclaimcount();
  }


  policiesbytypestatuslabel: string[] = []
  policiesbytypestatusdata: number[] = []

  policiesbytypestatus() {
    this.claimsOfficerReportService.policiesbytypestatus().subscribe({
      next: (data) => {
        data.forEach(element => {
          this.policiesbytypestatuslabel.push(element.planType)
          this.policiesbytypestatusdata.push(element.policyCount)

        });

        new Chart('pieChart', {

          type: 'pie',
          data: {
            labels: this.policiesbytypestatuslabel,
            datasets: [{
              data: this.policiesbytypestatusdata,
              backgroundColor: ['green', 'red', 'orange']
            }]
          }
        });
      },
      error: () => { }
    });
  }

  premiumvspayout() {
    this.claimsOfficerReportService.premiumvspayout().subscribe({
      next: (data) => {


        new Chart('pieChartPremiumvsPayout', {

          type: 'pie',
          data: {
            labels: ['Premium Collections', 'Claim Payout'],
            datasets: [{
              data: [data.totalPremiumCollected, data.totalClaimPayout],
              backgroundColor: ['green', 'red', 'orange']
            }]
          }
        });
      },
      error: () => { }
    });
  }

  planwisepolicyclaimcountlabel: string[] = []
  planwisepolicyclaimcountdata_claim: number[] = []
  planwisepolicyclaimcountdata_policy: number[] = []

  planwisepolicyclaimcount() {
    this.claimsOfficerReportService.planwisepolicyclaimcount().subscribe({
      next: (data) => {
        data.forEach(element => {
          this.planwisepolicyclaimcountlabel.push(element.planName)
          this.planwisepolicyclaimcountdata_claim.push(element.claimCount)
          this.planwisepolicyclaimcountdata_policy.push(element.policyCount)

        });


        new Chart('barChartPlanwisePolicy', {

          type: 'bar',
          data: {
            labels: this.planwisepolicyclaimcountlabel,
            datasets: [
              { data: this.planwisepolicyclaimcountdata_policy, label: 'Policy', backgroundColor: ['green'] },
              { data: this.planwisepolicyclaimcountdata_claim, label: 'Claims', backgroundColor: ['red'] },
            ],
          }
        });
      },
      error: () => { }
    });
  }

}


