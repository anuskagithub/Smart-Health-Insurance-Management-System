import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomerPlansService } from '../../services/customer-plans.service';

@Component({
  standalone: true,
  imports: [CommonModule],
  templateUrl: './fake-gateway.html'
})
export class FakeGatewayComponent implements OnInit {

  policyId!: number;
  amount!: number;
  insurancePlanId!: number;


  processing = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private service: CustomerPlansService
  ) { }

  ngOnInit(): void {
    this.policyId = Number(this.route.snapshot.queryParamMap.get('policyId'));
    this.amount = Number(this.route.snapshot.queryParamMap.get('amount'));
    this.insurancePlanId = Number(this.route.snapshot.queryParamMap.get('insurancePlanId'));
  }

  // Simulate successful payment
  confirmPayment(): void {
    if (this.processing) return;
    this.processing = true;

    const payload = {
      policyId: this.policyId,
      insurancePlanId: this.insurancePlanId,
      amount: this.amount,
      paymentMethod: 'FAKE_GATEWAY',
      isGatewayConfirmed: true
    };

    setTimeout(() => {
      this.service.payPremium(payload).subscribe({
        next: () => {
          alert('Payment Successful');
          this.router.navigate(['/customer/pages/my-policies/my-policies']);
        },
        error: () => {
          this.processing = false;
          alert('Payment confirmation failed');
        }
      });
    }, 1500);

  }

  // Optional cancel flow
  cancel(): void {
    this.router.navigate(['/customer/plans']);
  }
}
