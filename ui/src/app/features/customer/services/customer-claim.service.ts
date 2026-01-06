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
export class CustomerClaimsService {

    private readonly baseUrl = 'https://localhost:7200/api/Claim';

    constructor(private http: HttpClient) { }



    // Pay premium (first time or renewal)
    submitClaims(payload: any): Observable<any> {
        return this.http.post(`${this.baseUrl}/my-claims`, payload);
    }

    
}
