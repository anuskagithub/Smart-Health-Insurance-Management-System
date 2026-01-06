import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface PayPremiumPayload {
  policyId: number;
  amount: number;
  paymentMethod: string;       // REQUIRED
  isGatewayConfirmed: boolean; // REQUIRED for fake gateway flow
}

@Injectable({
  providedIn: 'root'
})
export class CustomerPlansService {

  private readonly baseUrl = 'https://localhost:7200/api/customer';

  constructor(private http: HttpClient) { }

  // View available plans
  getAvailablePlans(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/plans`);
  }

  // View purchased policies
  getMyPolicies(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/policies`);
  }

  // Pay premium (first time or renewal)
  payPremium(payload: PayPremiumPayload): Observable<any> {
    return this.http.post(`${this.baseUrl}/pay-premium`, payload);
  }
  getMyClaims(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/claims`);
  }
}
