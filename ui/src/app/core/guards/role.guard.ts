import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

export const roleGuard = (expectedRole: string): CanActivateFn => {
  return () => {
    const auth = inject(AuthService);
    const router = inject(Router);

    if (auth.isLoggedIn() && auth.hasRole(expectedRole)) {
      return true;
    }

    router.navigate(['/auth/login']);
    return false;
  };
};
