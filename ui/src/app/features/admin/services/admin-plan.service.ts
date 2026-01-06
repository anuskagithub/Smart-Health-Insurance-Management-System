import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface InsurancePlan {
  insurancePlanId: number;
  planName: string;
  planType: string;
  premiumAmount: number;
  coverageAmount: number;
  durationInMonths: number;
  isActive: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class AdminPlanService {

  private baseUrl = 'https://localhost:7200/api/admin/plans';

  constructor(private http: HttpClient) {}

  getPlans(): Observable<InsurancePlan[]> {
    return this.http.get<InsurancePlan[]>(this.baseUrl);
  }

  createPlan(plan: Partial<InsurancePlan>) {
    return this.http.post(this.baseUrl, plan);
  }

  updatePlan(planId: number, plan: Partial<InsurancePlan>) {
    return this.http.put(`${this.baseUrl}/${planId}`, plan);
  }

  disablePlan(planId: number) {
    return this.http.patch(`${this.baseUrl}/${planId}/disable`, {});
  }

  togglePlanStatus(planId: number, isActive: boolean) {
  return this.http.patch(
    `${this.baseUrl}/${planId}/status`,   // ✅ FIXED
    null,
    {
      params: {
        isActive: isActive.toString()      // ✅ ALSO REQUIRED
      }
    }
  );
}
}
