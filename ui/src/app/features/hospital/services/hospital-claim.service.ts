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
export class HospitalClaimService {

    private readonly baseUrl = 'https://localhost:7200/api/Claim';

    constructor(private http: HttpClient) { }

    treatmentUpdate(claimId: number , treatmentHistory: string): Observable<any> {
        return this.http.put(`${this.baseUrl}/hospital-treatmnt?claimId=${claimId}&treatmentHistory=${treatmentHistory}`, null);
    }
}
