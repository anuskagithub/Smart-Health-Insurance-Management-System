import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';

@Component({
  selector: 'app-admin-sidebar',
  standalone: true,
  imports: [
    RouterModule,
    MatIconModule,
    MatListModule
  ],
  templateUrl: './admin-sidebar.html',
  styleUrls: ['./admin-sidebar.scss']
})
export class AdminSidebarComponent {}
