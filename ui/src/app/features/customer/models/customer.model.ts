export interface ClaimList {
    claimId: number;
    policyNumber: string;
    providerName: string;
    claimAmount: number;
    status: string;
    submittedOn: Date;
    remarks: string;
    treatmentHistory: string;
    claimOfficerComment: string;
}

