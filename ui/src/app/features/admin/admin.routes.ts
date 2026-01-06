import { Routes } from '@angular/router';
import { AdminLayoutComponent } from './layout/admin-layout/admin-layout';

export const ADMIN_ROUTES: Routes = [
  {
    path: '',
    component: AdminLayoutComponent,
    children: [

      // DASHBOARD
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./pages/dashboard/dashboard')
            .then(m => m.DashboardComponent)
      },

      // USER APPROVAL
      {
        path: 'user-approval',
        loadComponent: () =>
          import('./pages/user-approval/user-approval')
            .then(m => m.UserApprovalComponent)
      },

      // MANAGE USERS (CRUD)
      //{
      //  path: 'manage-users',
      //  loadComponent: () =>
      //    import('./pages/manage-users/manage-users')
      //      .then(m => m.ManageUsersComponent)
      //},

      // PLANS
      {
        path: 'plans',
        loadComponent: () =>
          import('./pages/plans/plans')
            .then(m => m.InsurancePlansComponent)
      },

      // HOSPITALS
      {
        path: 'hospitals.page',
        loadComponent: () =>
          import('./pages/hospital/hospitals.page')
            .then(m => m.HospitalsPage)
      },

      //AGENT
      {
        path: 'agent.page',
        loadComponent: () =>
          import('./pages/agent/agent.page')
            .then(m => m.AgentPage)
      },

      {
        path: 'claimsofficer.page',
        loadComponent: () =>
          import('./pages/claimsofficer/claimsofficer.page')
            .then(m => m.ClaimOfficerPage)
      },

      // DEFAULT REDIRECT
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      }
    ]
  }
];
