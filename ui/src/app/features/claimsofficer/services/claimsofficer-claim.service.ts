import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ReviewClaimPayload {
    claimId: number;
    status: string;
    remarks: string;
}

@Injectable({
    providedIn: 'root'
})
export class ClaimsOfficerClaimService {

    private readonly baseUrl = 'https://localhost:7200/api/Claim';

    constructor(private http: HttpClient) { }

    claimsReview(reviewClaimPayload: ReviewClaimPayload): Observable<any> {
        return this.http.put(`${this.baseUrl}/Claim-Approve-Reject?claimId=${reviewClaimPayload.claimId}&status=${reviewClaimPayload.status}`, reviewClaimPayload);
    }

    getClaimsbyId(claimId: number): Observable<any> {
        return this.http.get<any>(`${this.baseUrl}/${claimId}`);
    }

}
