import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AdminUser } from '../models/admin-user.model';
import { HospitalProvider } from '../models/hospital-provider.model';

@Injectable({ providedIn: 'root' })
export class AdminService {
  private baseUrl = 'https://localhost:7200/api/admin';

  constructor(private http: HttpClient) {}

  getAllUsers(): Observable<AdminUser[]> {
    return this.http.get<AdminUser[]>(`${this.baseUrl}/users`);
  }

  approveUser(userId: string): Observable<any> {
    return this.http.put(`${this.baseUrl}/users/${userId}/approve`, {});
  }

  assignRole(userId: string, roles: string[]): Observable<any> {
    return this.http.put(
      `${this.baseUrl}/users/${userId}/roles`,
      roles
    );
  }

  getAllHospitals(): Observable<HospitalProvider[]> {
    return this.http.get<HospitalProvider[]>(`${this.baseUrl}/hospital-providers`);
  }
}
