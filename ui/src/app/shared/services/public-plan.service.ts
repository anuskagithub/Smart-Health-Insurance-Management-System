import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { InsurancePlan } from '../models/insurance-plan.model';

@Injectable({ providedIn: 'root' })
export class PublicPlanService {

  private baseUrl = 'https://localhost:7200/api/admin/plans';

  constructor(private http: HttpClient) {}

  getPlansByType(type: string): Observable<InsurancePlan[]> {
    return this.http.get<InsurancePlan[]>(this.baseUrl).pipe(
      map(plans =>
        plans.filter(p =>
          p.isActive &&
          p.planType.toLowerCase() === type.toLowerCase()
        )
      )
    );
  }
}
