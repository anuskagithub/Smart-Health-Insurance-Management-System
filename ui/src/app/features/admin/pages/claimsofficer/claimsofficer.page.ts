import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';

import { ClaimOfficerService } from './claimsofficer.service';
import { ClaimOfficerDialogComponent } from './claimsofficer.dialog';

@Component({
  standalone: true,
  templateUrl: './claimsofficer.page.html',
  imports: [
    CommonModule,
    MatTableModule,
    MatPaginatorModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule
  ]
})
export class ClaimOfficerPage implements OnInit {

  displayedColumns = [
    'employeeCode',
    'fullName',
    'department',
    'approvedClaimsCount',
    'isActive',
    'actions'
  ];

  dataSource = new MatTableDataSource<any>([]);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(
    private service: ClaimOfficerService,
    private dialog: MatDialog
  ) {}

  ngOnInit() {
    this.load();
  }

  load() {
    this.service.getAll().subscribe(res => {
      this.dataSource.data = res;
      this.dataSource.paginator = this.paginator;
    });
  }

  addOfficer() {
    this.dialog.open(ClaimOfficerDialogComponent)
      .afterClosed()
      .subscribe(ok => ok && this.load());
  }

  editOfficer(officer: any) {
    this.dialog.open(ClaimOfficerDialogComponent, {
      data: {
        id: officer.claimsOfficerProfileId,
        fullname: officer.fullname,
        email: officer.email,
        phoneNumber: officer.phoneNumber,
        address: officer.address,
        dateOfBirth: officer.dateOfBirth,        
        employeeCode: officer.employeeCode,
        department: officer.department
      }
    })
    .afterClosed()
    .subscribe(ok => ok && this.load());
  }
}
