import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class HospitalService {

  private readonly baseUrl = 'https://localhost:7200/api/admin/hospital-providers';

  constructor(private http: HttpClient) {}

  getAll() {
    return this.http.get<any[]>(this.baseUrl);
  }

  create(payload: any) {
    return this.http.post(this.baseUrl, payload);
  }

  update(id: number, payload: any) {
    return this.http.put(`${this.baseUrl}/${id}`, payload);
  }

  toggleNetwork(id: number, isNetwork: boolean) {
    const params = new HttpParams().set('isNetwork', isNetwork);
    return this.http.put(`${this.baseUrl}/${id}/network`, {}, { params });
  }

  toggleActive(id: number, isActive: boolean) {
    const params = new HttpParams().set('isActive', isActive);
    return this.http.put(`${this.baseUrl}/${id}/active`, {}, { params });
  }
}
