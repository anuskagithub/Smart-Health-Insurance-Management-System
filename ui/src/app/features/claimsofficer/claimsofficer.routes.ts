import { Routes } from '@angular/router';
import { ClaimsOfficerLayoutComponent } from './pages/layout/claimsofficer-layout';
import { ClaimsOfficerDashboardComponent } from './pages/dashboard/dashboard';
import { ViewClaimsComponent } from './pages/view-claims/view-claims';


export const CLAIMSOFFICER_ROUTES: Routes = [
  {
    path: '',
    component: ClaimsOfficerLayoutComponent,
    children: [
      { path: 'dashboard', component: ClaimsOfficerDashboardComponent},
      {path: 'view-claims',  component: ViewClaimsComponent},
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
    ]
  }
];
