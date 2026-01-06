import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.html',
  styleUrls: ['./login.scss']
})
export class LoginComponent {

  username = '';
  password = '';
  selectedRole = '';
  error = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  login() {
  this.error = '';

  if (!this.selectedRole) {
    this.error = 'Please select a role to login';
    return;
  }

  const payload = {
    Username: this.username,
    Password: this.password
  };

  this.authService.login(payload).subscribe({
    next: (res) => {
      const backendRole = res.roles?.[0];

      // 🔒 ROLE MUST MATCH
      if (backendRole !== this.selectedRole) {
        this.error = `You are not authorized to login as ${this.selectedRole}`;
        return;
      }

      // 🔒 TEMPORARILY ALLOW ONLY ADMIN + CUSTOMER
      //if (backendRole !== 'Admin' && backendRole !== 'Customer') {
        //this.error = 'This role is not enabled yet';
        //return;
      //}

      this.authService.setAuth(res.token, backendRole);

      // 🔁 REDIRECT
      if (backendRole === 'Admin') {
        this.router.navigate(['/admin/dashboard']);
      } else if (backendRole === 'Customer') {
        this.router.navigate(['/customer/plans']);
      } else if (backendRole === 'Agent' ) {
        this.router.navigate(['/agent/dashboard']);
      } else if (backendRole === 'ClaimsOfficer' ) {
        this.router.navigate(['/claimsofficer/dashboard']);
      } else if (backendRole === 'HospitalProvider' ) {
        this.router.navigate(['/hospital/view-claims']);
      }
      
    },
    error: () => {
      this.error = 'Invalid credentials or not approved';
    }
  });
}

}