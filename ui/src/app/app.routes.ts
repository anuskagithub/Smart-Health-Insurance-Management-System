import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home';
import { AUTH_ROUTES } from './auth/auth.routes';
import { ADMIN_ROUTES } from './features/admin/admin.routes';
import { adminGuard } from './core/guards/admin.guard';
import { roleGuard } from './core/guards/role.guard';


export const appRoutes: Routes = [

  // HOME (MATCH ONLY '/')
  {
    path: '',
    component: HomeComponent,
    pathMatch: 'full'   // 🔥 THIS FIXES EVERYTHING
  },
  // AUTH
  {
    path: 'auth',
    children: AUTH_ROUTES
  },

  {
  path: 'customer',
  canActivate: [roleGuard('Customer')],
  loadChildren: () =>
    import('./features/customer/customer.routes')
      .then(m => m.CUSTOMER_ROUTES)
},

{
  path: 'hospital',
  canActivate: [roleGuard('HospitalProvider')],
  loadChildren: () =>
    import('./features/hospital/hospital.routes')
      .then(m => m.HOSPITAL_ROUTES)
},

{
  path: 'claimsofficer',
  canActivate: [roleGuard('ClaimsOfficer')],
  loadChildren: () =>
    import('./features/claimsofficer/claimsofficer.routes')
      .then(m => m.CLAIMSOFFICER_ROUTES)
},

  // ADMIN
  {
    path: 'admin',
    canActivate: [adminGuard],
    children: ADMIN_ROUTES
  },

  // FALLBACK
  {
    path: '**',
    redirectTo: ''
  }
];
