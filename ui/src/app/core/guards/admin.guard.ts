import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

export const adminGuard: CanActivateFn = () => {
  const auth = inject(AuthService);
  const router = inject(Router);

  // 🔥 SIGNAL READ (SYNC + RELIABLE)
  if (auth.isAdmin()) {
    return true;
  }

  router.navigate(['/auth/login']);
  return false;
};
