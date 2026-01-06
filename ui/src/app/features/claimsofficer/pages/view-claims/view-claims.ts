import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ClaimList } from '../../../customer/models/customer.model';
import { MatDialog } from '@angular/material/dialog';
import { ViewClaimsDialogComponent } from './view-claims-dialog';
import { ClaimsOfficerService } from '../../services/claimsofficer.service';
import { ClaimsOfficerClaimService } from '../../services/claimsofficer-claim.service';



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
    'actions'
  ];

  dataSource = new MatTableDataSource<ClaimList>([]);
  loading = false;

  showPendingOnly = false;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(private claimsOfficerService: ClaimsOfficerService, private dialog: MatDialog,
    private claimsOfficerClaimService: ClaimsOfficerClaimService
  ) { }

  ngOnInit(): void {
    this.claimList();
  }

  claimList() {
    this.loading = true;

    this.claimsOfficerService.getMyClaims().subscribe({
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

  review(claimId: number): void {
    this.getClaimIdbyId(claimId)
  }

  getClaimIdbyId(claimId: number): void {
    this.claimsOfficerClaimService.getClaimsbyId(claimId).subscribe({
      next: (data) => {
        this.dialog.open(ViewClaimsDialogComponent, {
          width: '500px',
          data: {claimId, claimDetails: data}
        }).afterClosed().subscribe(res => {
          if (res) this.claimList();
        });
      },
      error: () => { }
    });
  }
}
