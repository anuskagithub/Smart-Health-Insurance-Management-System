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
export class HospitalService {

  private readonly baseUrl = 'https://localhost:7200/api/HospitalProvider';

  constructor(private http: HttpClient) { }

  getMyClaims(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/claims`);
  }
}
