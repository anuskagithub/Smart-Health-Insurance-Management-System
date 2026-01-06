import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { HospitalClaimService } from '../../services/hospital-claim.service';


@Component({
  standalone: true,
  selector: 'app-view-claims-dialog',
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
  templateUrl: './view-claims-dialog.html',
  styleUrls: ['./view-claims-dialog.scss']
})
export class ViewClaimsDialogComponent implements OnInit {

  planForm!: FormGroup;
  isEdit = false;

  constructor(
    private fb: FormBuilder,
    private hospitalClaimService: HospitalClaimService,
    private dialogRef: MatDialogRef<ViewClaimsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public claimId: number
  ) {}

  ngOnInit(): void {

    this.planForm = this.fb.group({
      treatmentHistory: ['', Validators.required],
    });
  }

  save(): void {
    if (this.planForm.invalid) {
      this.planForm.markAllAsTouched();
      return;
    }

    const payload = this.planForm.value.treatmentHistory;

    const request$ = this.hospitalClaimService.treatmentUpdate(this.claimId, payload);

    request$.subscribe(() => this.dialogRef.close(true));
  }

  cancel(): void {
    this.dialogRef.close();
  }
}
