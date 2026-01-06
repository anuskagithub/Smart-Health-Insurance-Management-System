export interface InsurancePlan {
  planId: number;
  planName: string;
  planType: string;
  premiumAmount: number;
  coverageAmount: number;
  durationInMonths: number;
  isActive: boolean;
}