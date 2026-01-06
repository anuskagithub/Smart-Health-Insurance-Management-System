import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';

import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';

import { HospitalService } from './hospital.service';

@Component({
  standalone: true,
  templateUrl: './hospital-dialog.html',
  styleUrl: './hospital-dialog.scss',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatInputModule,
    MatButtonModule,
    MatCheckboxModule
  ]
})
export class HospitalDialogComponent {

  form: FormGroup;
  isEdit = false;

  constructor(
    private fb: FormBuilder,
    private hospitalService: HospitalService,
    private dialogRef: MatDialogRef<HospitalDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.isEdit = !!data;

    this.form = this.fb.group({
      // ---- USER / LOGIN INFO ----
      username: ['', Validators.required],
      password: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', Validators.required],

      // ---- HOSPITAL INFO ----
      providerName: ['', Validators.required],
      providerType: ['', Validators.required],
      city: ['', Validators.required],
      isNetworkProvider: [true]
    });

    if (this.isEdit) {
      this.form.patchValue(data);
      // Disable user fields
      this.form.get('username')?.disable();
      this.form.get('password')?.disable();
      this.form.get('email')?.disable();
      this.form.get('phoneNumber')?.disable();

      // REMOVE validators so form becomes valid
      this.form.get('username')?.clearValidators();
      this.form.get('password')?.clearValidators();
      this.form.get('email')?.clearValidators();
      this.form.get('phoneNumber')?.clearValidators();

      this.form.get('username')?.updateValueAndValidity();
      this.form.get('password')?.updateValueAndValidity();
      this.form.get('email')?.updateValueAndValidity();
      this.form.get('phoneNumber')?.updateValueAndValidity();
    }
  }

  save(): void {
    console.log('SAVE CLICKED', this.form.value, this.form.valid);
    if (this.form.invalid) return;

    if (!this.isEdit) {

      const regPayload = {
        Username: this.form.value.username,
        Email: 'test@gmail.com',
        Password: this.form.value.password,
        FullName: this.form.value.providerName,
        PhoneNumber: this.form.value.phoneNumber,
        Address: this.form.value.city,
        DateOfBirth: '1990-01-01'
      };

      const hostPayload = {
        userId: '',
        ProviderName: this.form.value.providerName,
        ProviderType: this.form.value.providerType,
        City: this.form.value.city,
        IsNetworkProvider: this.form.value.isNetworkProvider
      };

      const payload = {
        RegisterInfo: regPayload,
        HospitalProviderInfo: hostPayload
      };

      console.log('FINAL PAYLOAD SENT', payload);

      this.hospitalService.create(payload).subscribe({
        next: res => {
          console.log('CREATE SUCCESS', res);
          this.dialogRef.close(true);
        },
        error: err => {
          console.error('CREATE FAILED', err.error || err);
        }
      });

    } else {
  const value = this.form.getRawValue();

  const payload = {
    providerName: value.providerName,
    providerType: value.providerType,
    city: value.city,
    isNetworkProvider: value.isNetworkProvider
  };

  this.hospitalService
    .update(this.data.id, payload)
    .subscribe(() => this.dialogRef.close(true));
}
  }

  close(): void {
    this.dialogRef.close();
  }
}
