import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

import { AdminService } from '../../services/admin.service';
import { AdminUser } from '../../models/admin-user.model';

@Component({
  selector: 'app-user-approval',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatButtonModule,
    MatSelectModule,
    MatFormFieldModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './user-approval.html',
  styleUrls: ['./user-approval.scss']
})
export class UserApprovalComponent implements OnInit {

  displayedColumns: string[] = [
    'userName',
    'email',
    'registeredOn',
    'status',
    'role',
    'actions'
  ];

  dataSource = new MatTableDataSource<AdminUser>([]);
  loading = false;

  roles: string[] = [
    'Admin',
    'Customer',
    'Agent',
    'ClaimsOfficer',
    'HospitalProvider'
  ];

  selectedRoles: { [userId: string]: string } = {};
  showPendingOnly = false;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(private adminService: AdminService) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers() {
    this.loading = true;

    this.adminService.getAllUsers().subscribe({
      next: (users) => {
        this.dataSource.data = users;
        this.dataSource.paginator = this.paginator;
        this.applyFilter();
        this.loading = false;
      },
      error: () => (this.loading = false)
    });
  }

  approve(userId: string) {
    this.adminService.approveUser(userId).subscribe(() => {
      this.loadUsers();
    });
  }

  assignRole(userId: string) {
    const role = this.selectedRoles[userId];
    if (!role) return;

    this.adminService.assignRole(userId, [role]).subscribe(() => {
      this.loadUsers();
    });
  }

  togglePendingFilter() {
    this.showPendingOnly = !this.showPendingOnly;
    this.applyFilter();
  }

  applyFilter() {
    this.dataSource.filterPredicate = (user) =>
      this.showPendingOnly ? !user.isApproved : true;

    this.dataSource.filter = Math.random().toString();
  }
}
