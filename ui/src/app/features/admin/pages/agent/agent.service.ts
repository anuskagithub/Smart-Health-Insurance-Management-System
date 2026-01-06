import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class AgentService {

private readonly baseUrl = 'https://localhost:7200/api/admin/agents';


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

}

