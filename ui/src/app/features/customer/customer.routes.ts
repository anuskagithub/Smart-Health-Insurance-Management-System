import { Routes } from '@angular/router';
import { CustomerLayoutComponent } from './layout/customer-layout';
import { CustomerDashboardComponent } from './pages/dashboard/dashboard';
import { CustomerPlansComponent } from './pages/plans/plans';
import { PayPremiumComponent } from './pages/pay-premiun/pay-premium';
import { MyPoliciesComponent } from './pages/my-polices/my-policies';
import { FakeGatewayComponent } from './pages/fake-gateway/fake-gateway';
import { SubmitClaimComponent } from './pages/submit-claim/submit-claim';
import { ViewClaimsComponent } from './pages/view-claims/view-claims';

export const CUSTOMER_ROUTES: Routes = [
  {
    path: '',
    component: CustomerLayoutComponent,
    children: [
      { path: 'dashboard', component: CustomerDashboardComponent },
      { path: 'plans', component: CustomerPlansComponent },
      { path: 'pay-premiun', component: PayPremiumComponent },
      { path: 'my-polices', component: MyPoliciesComponent },
      { path: 'fake-gateway', component: FakeGatewayComponent },
      { path: 'submit-claim', component: SubmitClaimComponent },
      { path: 'view-claims', component: ViewClaimsComponent },
      { path: '', redirectTo: 'plans', pathMatch: 'full' }
    ]
  }
];
