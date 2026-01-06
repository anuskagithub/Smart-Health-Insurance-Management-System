import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ClaimList } from '../../models/customer.model';
import { CustomerPlansService } from '../../services/customer-plans.service';


@Component({
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
  templateUrl: './view-claims.html',
  styleUrls: ['./view-claims.scss']
})
export class ViewClaimsComponent implements OnInit {

  displayedColumns: string[] = [
    'policyNumber',
    'providerName',
    'claimAmount',
    'status',
    'submittedOn',
    'remarks',
    'treatmentHistory',
    'claimOfficerComment'
  ];

  dataSource = new MatTableDataSource<ClaimList>([]);
  loading = false;

  showPendingOnly = false;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(private customerPlansService: CustomerPlansService ) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers() {
    this.loading = true;

    this.customerPlansService.getMyClaims().subscribe({
      next: (data) => {
        this.dataSource.data = data;
        this.dataSource.paginator = this.paginator;
        this.loading = false;
      },
      error: () => (this.loading = false)
    });
  }

  togglePendingFilter() {
    this.showPendingOnly = !this.showPendingOnly;
  }
}
