import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
import { HospitalProvider } from '../../../admin/models/hospital-provider.model';
import { CustomerClaimsService } from '../../services/customer-claim.service';
import { CustomerPolicyService } from '../../services/customer-policy.service';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { CustomerPlansService } from '../../customer-plans.service';

@Component({
    standalone: true,
    imports: [CommonModule,
        MatSelectModule,
        ReactiveFormsModule,
        MatInputModule,
        MatButtonModule],
    templateUrl: './submit-claim.html',
    styleUrls: ['./submit-claim.scss']
})
export class SubmitClaimComponent implements OnInit {

    form: FormGroup;
    policyId!: number;

    hospitalProviders: HospitalProvider[] = [];
    myPolicies: any;
    constructor(
        private fb: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private customerPlansService: CustomerPlansService,
        private customerClaimsService: CustomerClaimsService,
        private customerPolicyService: CustomerPolicyService

    ) {
        this.form = this.fb.group({

            claimamount: ['', Validators.required],
            remarks: ['', Validators.required],
            selectedHospitalProviderId: ['', Validators.required]
        });
    }

    ngOnInit(): void {
        this.policyId = Number(this.route.snapshot.queryParamMap.get('policyId'));
        this.getAllHospitals();
        this.getMyPolicies();
    }

    getAllHospitals(): void {
        this.customerPlansService.getAllHospitals().subscribe({
            next: (data) => {
                console.log('HOSPITAL PROVIDERS', data);
                this.hospitalProviders = data;
            },
            error: (err) => {
                console.error('Error loading hospital providers', err);
            }
        });
    }

    getMyPolicies(): void {
        this.customerPolicyService.getMyPolicies(this.policyId).subscribe({
            next: (data) => {
                console.log('GET MY POLICIES', data);
                this.myPolicies = data;
            },
            error: (err) => {
                console.error('Error loading hospital providers', err);
            }
        });
    }

    claimPolicy(policyId: number): void {
        console.log('SAVE CLICKED', this.form.value, this.form.valid);
        if (this.form.invalid) return;


        const claimPayload = {
            policyId: this.policyId,
            hospitalProviderId: this.form.value.selectedHospitalProviderId,
            claimAmount: this.form.value.claimamount,
            remarks: this.form.value.remarks

        };
        this.customerClaimsService.submitClaims(claimPayload).subscribe({
            next: (data) => {
                this.router.navigate(['/customer/view-claims'], {
                });

            },
            error: (err) => {
                console.error('Error loading hospital providers', err);
            }
        });
    }
}

