import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CustomerPlansService } from '../../services/customer-plans.service';
import { Router } from '@angular/router';

@Component({
  standalone: true,
  imports: [CommonModule],
  templateUrl: './my-policies.html',
  styleUrls: ['./my-policies.scss']
})
export class MyPoliciesComponent implements OnInit {

  policies: any[] = [];
  loading = true;

  constructor(private service: CustomerPlansService, private router: Router) {

  }

  ngOnInit(): void {
    this.service.getMyPolicies().subscribe({
      next: (data: any[]) => {
        this.policies = data;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        alert('Failed to load policies');
      }
    });
  }

  payPremium(policyId: number): void {
    const payload = {
      policyId,
      amount: 0,
      paymentMethod: 'UPI',
      isGatewayConfirmed: true
    };

    this.service.payPremium(payload).subscribe({
      next: () => alert('Premium paid successfully'),
      error: () => alert('Premium payment failed')
    });
  }

  claimPolicy(policyId: number): void {
    this.router.navigate(['/customer/submit-claim'], {
      queryParams: {
        policyId: policyId
      }
    });
  }
}   
