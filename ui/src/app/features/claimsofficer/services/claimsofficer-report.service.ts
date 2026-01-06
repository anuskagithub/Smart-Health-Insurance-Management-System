import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface PolicyStatusResponse {
    planType: string;
    policyCount: number;
}

export interface PaymentvsPayoutResponse {
    totalPremiumCollected: number;
    totalClaimPayout: number;
}

export interface PlanwisePolicyClaimCountResponse {
    planName: string;
    policyCount: number;
    claimCount: number;
}

@Injectable({
    providedIn: 'root'
})
export class ClaimsOfficerReportService {

    private readonly baseUrl = 'https://localhost:7200/api/Reports';

    constructor(private http: HttpClient) { }

    policiesbytypestatus(): Observable<PolicyStatusResponse[]> {
        return this.http.get<PolicyStatusResponse[]>(`${this.baseUrl}/policies-by-type-status`);
    }

    premiumvspayout(): Observable<PaymentvsPayoutResponse> {
        return this.http.get<PaymentvsPayoutResponse>(`${this.baseUrl}/premium-vs-payout`);
    }

    planwisepolicyclaimcount(): Observable<PlanwisePolicyClaimCountResponse[]> {
        return this.http.get<PlanwisePolicyClaimCountResponse[]>(`${this.baseUrl}/Plan-Wise-PolicyClaimCount`);
    }


}