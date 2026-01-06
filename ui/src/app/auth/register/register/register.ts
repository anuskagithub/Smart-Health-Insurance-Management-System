import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule
} from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './register.html',
  styleUrls: ['./register.scss']
})
export class RegisterComponent {

  registerForm: FormGroup;
  submitting = false;
  successMessage = '';
  errorMessage = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.registerForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      fullName: ['', Validators.required],
      phoneNumber: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
      address: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      nomineeName: ['', Validators.required]
    });
  }

  // ---------- SAFE GETTERS ----------
  get f() {
    return this.registerForm.controls;
  }

  // ---------- SUBMIT ----------
  submit(): void {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this.submitting = true;
    this.successMessage = '';
    this.errorMessage = '';

    const regPayload = {
      Username: this.f['username'].value,
      Email: this.f['email'].value,
      Password: this.f['password'].value,
      FullName: this.f['fullName'].value,
      PhoneNumber: this.f['phoneNumber'].value,
      Address: this.f['address'].value,
      DateOfBirth: this.f['dateOfBirth'].value
    };

    const custPayload = {
      FullName: this.f['fullName'].value,
      PhoneNumber: this.f['phoneNumber'].value,
      Address: this.f['address'].value,
      DateOfBirth: this.f['dateOfBirth'].value,
      NomineeName: this.f['nomineeName'].value
     };

    const payload = {
      RegisterInfo : regPayload,
      CustProfileInfo: custPayload
    };  

    console.log('Register payload:', payload);

    this.authService.register(payload).subscribe({
      next: (res: string) => {
        this.successMessage = res;
        this.submitting = false;
        this.registerForm.reset();
      },
      error: (err) => {
        this.errorMessage =
          err?.error ?? 'Registration failed. Please try again.';
        this.submitting = false;
      }
    });
  }
}
