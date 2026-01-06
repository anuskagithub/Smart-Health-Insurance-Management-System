import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HospitalProvider } from '../admin/models/hospital-provider.model';

@Injectable({ providedIn: 'root' })
export class CustomerPlansService {

  private baseUrl = 'https://localhost:7200/api/customer';

  constructor(private http: HttpClient) {}

  getAvailablePlans(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/plans`);
  }

  payPremium(payload: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/pay-premium`, payload);
  }

  // STEP 4: View purchased policies
  getMyPolicies(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/policies`);
  }

  getAllHospitals(): Observable<HospitalProvider[]> {
      return this.http.get<HospitalProvider[]>(`${this.baseUrl}/hospital-providers`);
    }



}
