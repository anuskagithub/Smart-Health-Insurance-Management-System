import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';

import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

import { AgentService } from './agent.service';

@Component({
  standalone: true,
  templateUrl: './agent-dialog.html',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule
  ]
})
export class AgentDialogComponent {

  form: FormGroup;
  isEdit = false;

  constructor(
    fb: FormBuilder,
    private service: AgentService,
    private dialogRef: MatDialogRef<AgentDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.isEdit = !!data?.id;

    this.form = fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],

      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', Validators.required],
      address: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      agentCode: ['', Validators.required],
      yearsOfExperience: [0, Validators.required],
      region: ['', Validators.required],
      isActive: [true]
    });

    if (this.isEdit) {
      this.form.patchValue({
        fullName : data.fullName,
        agentCode: data.agentCode,
        address: data.address,
        email: data.email,
        phoneNumber: data.phoneNumber
      });


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
          Username: this.form.value.username,
          Email: this.form.value.email,
          Password: this.form.value.password,
          FullName: this.form.value.fullName,
          PhoneNumber: this.form.value.phoneNumber,
          Address: this.form.value.address,
          DateOfBirth: this.form.value.dateOfBirth
        },
        agentProfileInfo: {
          userId: '',
          AgentCode: this.form.value.agentCode,
          YearsOfExperience: this.form.value.yearsOfExperience,
          Region: this.form.value.region
        }
      };

      this.service.create(payload)
        .subscribe(() => this.dialogRef.close(true));
    }

    // UPDATE
    else {
      const v = this.form.getRawValue();

      const payload = {
        yearsOfExperience: v.yearsOfExperience,
        region: v.region,
        isActive: v.isActive
      };

      this.service.update(this.data.id, payload)
        .subscribe(() => this.dialogRef.close(true));
    }
  }

  close() {
    this.dialogRef.close();
  }
}
