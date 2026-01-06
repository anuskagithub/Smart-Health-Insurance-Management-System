import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class ClaimOfficerService {

  private readonly baseUrl = 'https://localhost:7200/api/admin/claimofficer';

  constructor(private http: HttpClient) {}

  // GET → list
  getAll() {
    return this.http.get<any[]>(this.baseUrl);
  }

  // POST → create
  create(payload: any) {
    return this.http.post(this.baseUrl, payload);
  }

  // PUT → update
  update(id: number, payload: any) {
    return this.http.put(`${this.baseUrl}/${id}`, payload);
  }
}
