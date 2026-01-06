import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { ClaimsOfficerClaimService } from '../../services/claimsofficer-claim.service';
import { MatRadioModule } from '@angular/material/radio';


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
    MatButtonModule,
    MatRadioModule
  ],
  templateUrl: './view-claims-dialog.html',
  styleUrls: ['./view-claims-dialog.scss']
})
export class ViewClaimsDialogComponent implements OnInit {

  planForm!: FormGroup;
  isEdit = false;
  claimDetails: any;

  constructor(
    private fb: FormBuilder,
    private claimsOfficerClaimService: ClaimsOfficerClaimService,
    private dialogRef: MatDialogRef<ViewClaimsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { 
    this.claimDetails=this.data.claimDetails;
   }

  ngOnInit(): void {

    this.planForm = this.fb.group({
      remarks: ['', Validators.required],
      status: ['', Validators.required],
    });
  }
  


  save(): void {
    if (this.planForm.invalid) {
      this.planForm.markAllAsTouched();
      return;
    }

    const payload = {
      remarks: this.planForm.value.remarks,
      status: this.planForm.value.status,
      claimId: this.data.claimId

    }

    const request$ = this.claimsOfficerClaimService.claimsReview(payload);

    request$.subscribe(() => this.dialogRef.close(true));
  }

  cancel(): void {
    this.dialogRef.close();
  }
}

