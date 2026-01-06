import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ClaimsOfficerService {

  private readonly baseUrl = 'https://localhost:7200/api/ClaimsOfficer';

  constructor(private http: HttpClient) { }

  getMyClaims(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/ClaimsforClaimsOfficer`);
  }
}
