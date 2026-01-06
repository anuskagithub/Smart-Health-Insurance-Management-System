import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  canActivate(route: ActivatedRouteSnapshot): boolean {

    // 🔒 Step 1: Check authentication
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/auth/login']);
      return false;
    }

    // 🔒 Step 2: Check role authorization (if defined)
    const allowedRoles = route.data['roles'] as string[] | undefined;
    const userRole = this.authService.getRole();

    if (allowedRoles && (!userRole || !allowedRoles.includes(userRole))) {
      this.router.navigate(['/unauthorized']);
      return false;
    }

    // ✅ Access granted
    return true;
  }
}
