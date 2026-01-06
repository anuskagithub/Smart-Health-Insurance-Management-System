import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface PolicyEnrollPayload {
  insurancePlanId: number;
  customerProfileId: number;
  startDate: Date;

}

@Injectable({
  providedIn: 'root'
})
export class CustomerPolicyService {

  private readonly baseUrl = 'https://localhost:7200/api/Policy';

  constructor(private http: HttpClient) { }


  // Pay premium (first time or renewal)
  enrollPolicy(payload: PolicyEnrollPayload): Observable<any> {
    return this.http.post(`${this.baseUrl}/enroll`, payload);
  }

  getMyPolicies(policyId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/PolicyById?policyId=` + policyId);
  }



}
