import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';

import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

import { ClaimOfficerService } from './claimsofficer.service';

@Component({
  standalone: true,
  templateUrl: './claimsofficer.dialog.html',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule
  ]
})
export class ClaimOfficerDialogComponent {

  form: FormGroup;
  isEdit = false;

  constructor(
    fb: FormBuilder,
    private service: ClaimOfficerService,
    private dialogRef: MatDialogRef<ClaimOfficerDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.isEdit = !!data?.id;

    this.form = fb.group({
      // login (only on create)
      username: ['', Validators.required],
      password: ['', Validators.required],

      // claims officer
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', Validators.required],
      address: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      employeeCode: ['', Validators.required],
      department: ['', Validators.required]
    });

    if (this.isEdit) {
      this.form.patchValue(data);

      this.form.get('username')?.disable();
      this.form.get('password')?.disable();

      this.form.get('username')?.clearValidators();
      this.form.get('password')?.clearValidators();
    }
  }

  save() {
    if (this.form.invalid) return;

    // CREATE
    if (!this.isEdit) {
      const payload = {
        registerInfo: {
          username: this.form.value.username,
          password: this.form.value.password,
          email: this.form.value.email,
          fullName: this.form.value.fullName,
          phoneNumber: this.form.value.phoneNumber,
          address: this.form.value.address,
          dateOfBirth: this.form.value.dateOfBirth
        },
        ClaimsOfficerCreateInfo: {
          employeeCode: this.form.value.employeeCode,
          department: this.form.value.department
        }
      };

      this.service.create(payload)
        .subscribe(() => this.dialogRef.close(true));
    }

    // UPDATE
    else {
      const payload = {
        department: this.form.value.department
      };

      this.service.update(this.data.id, payload)
        .subscribe(() => this.dialogRef.close(true));
    }
  }

  close() {
    this.dialogRef.close();
  }
}
