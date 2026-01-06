import { Routes } from '@angular/router';
import { ViewClaimsComponent } from './pages/view-claims/view-claims';
import { HospitalLayoutComponent } from './pages/layout/hospital-layout';
import { HospitalDashboardComponent } from './pages/dashboard/dashboard';

export const HOSPITAL_ROUTES: Routes = [
  {
    path: '',
    component: HospitalLayoutComponent,
    children: [
      { path: 'dashboard', component: HospitalDashboardComponent },
      {path: 'view-claims',  component: ViewClaimsComponent},
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
    ]
  }
];
