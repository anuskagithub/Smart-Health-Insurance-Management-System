import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomerPlansService } from '../../services/customer-plans.service';

@Component({
  standalone: true,
  imports: [CommonModule],
  templateUrl: './pay-premium.html'
})
export class PayPremiumComponent implements OnInit {

  planId!: number;
  premiumAmount = 5000;
  policyId!: number;
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private service: CustomerPlansService
  ) { }

  ngOnInit(): void {
    this.planId = Number(this.route.snapshot.queryParamMap.get('planId'));
    this.policyId = Number(this.route.snapshot.queryParamMap.get('policyId'));
  }

  pay(): void {
    this.router.navigate(['/customer/fake-gateway'], {
      queryParams: {
        policyId: this.policyId,
        insurancePlanId: this.planId,
        amount: this.premiumAmount
      }
    });
  }
}
