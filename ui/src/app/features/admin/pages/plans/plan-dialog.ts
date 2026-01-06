import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';

import { AdminPlanService, InsurancePlan } from '../../services/admin-plan.service';

import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';


@Component({
  standalone: true,
  selector: 'app-plan-dialog',
  imports: [
    CommonModule,
    ReactiveFormsModule,

    // ✅ REQUIRED FOR mat-dialog-content, mat-dialog-actions
    MatDialogModule,

    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule
  ],
  templateUrl: './plan-dialog.html',
  styleUrls: ['./plan-dialog.scss']
})
export class PlanDialogComponent implements OnInit {

  planForm!: FormGroup;
  isEdit = false;

  constructor(
    private fb: FormBuilder,
    private planService: AdminPlanService,
    private dialogRef: MatDialogRef<PlanDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: InsurancePlan | null
  ) {}

  ngOnInit(): void {
    this.isEdit = !!this.data;

    this.planForm = this.fb.group({
      planName: ['', Validators.required],
      planType: ['', Validators.required],
      premiumAmount: [null, [Validators.required, Validators.min(1)]],
      coverageAmount: [null, [Validators.required, Validators.min(1)]],
      durationInMonths: [null, [Validators.required, Validators.min(1)]]
    });

    if (this.data) {
      this.planForm.patchValue({
        planName: this.data.planName,
        planType: this.data.planType,
        premiumAmount: this.data.premiumAmount,
        coverageAmount: this.data.coverageAmount,
        durationInMonths: this.data.durationInMonths
      });
    }
  }

  save(): void {
    if (this.planForm.invalid) {
      this.planForm.markAllAsTouched();
      return;
    }

    const payload = this.planForm.value;

    const request$ = this.isEdit
      ? this.planService.updatePlan(this.data!.insurancePlanId, payload)
      : this.planService.createPlan(payload);

    request$.subscribe(() => this.dialogRef.close(true));
  }

  cancel(): void {
    this.dialogRef.close();
  }
}
