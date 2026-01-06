import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { CustomerPlansService } from '../../services/customer-plans.service';
import { CustomerPolicyService } from '../../services/customer-policy.service';

@Component({
  selector: 'app-customer-plans',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './plans.html',
  styleUrls: ['./plans.scss']
})
export class CustomerPlansComponent implements OnInit {

  plans: any[] = [];
  loading = true;

  constructor(
    private plansService: CustomerPlansService,
    private policyService: CustomerPolicyService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.plansService.getAvailablePlans().subscribe({
      next: (data) => {
        this.plans = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading plans', err);
        this.loading = false;
        alert('Failed to load plans');
      }
    });
  }

  buy(planId: number) {
    const payload = {
      insurancePlanId: planId,
      customerProfileId: 0,
      startDate: new Date()
    };
    this.policyService.enrollPolicy(payload).subscribe({
      next: (data) => {
        console.log(data);
        this.router.navigate(['/customer/pay-premiun'], {
          queryParams: { planId, policyId : data['policyId'] }
        });
        this.plans = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading plans', err);
      }
    });
  }
}
