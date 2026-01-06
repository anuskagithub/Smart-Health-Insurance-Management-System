import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';

import { HospitalService } from './hospital.service';
import { HospitalDialogComponent } from './hospital-dialog';

@Component({
  standalone: true,
  selector: 'app-hospitals',
  imports: [
    CommonModule,
    MatTableModule,
    MatPaginatorModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatSlideToggleModule
  ],
  templateUrl: './hospitals.page.html',
  styleUrls: ['./hospitals.page.scss']
})
export class HospitalsPage implements OnInit {

  displayedColumns = [
    'providerName',
    'providerType',
    'city',
    'network',
    'active',
    'actions'
  ];

  dataSource = new MatTableDataSource<any>([]);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(
    private hospitalService: HospitalService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadHospitals();
  }

  loadHospitals(): void {
    this.hospitalService.getAll().subscribe(res => {
      this.dataSource.data = res;
      this.dataSource.paginator = this.paginator;
    });
  }

  // ➕ ADD
  addHospital(): void {
    this.dialog.open(HospitalDialogComponent, {
      width: '500px'
    }).afterClosed().subscribe(ok => {
      if (ok) this.loadHospitals();
    });
  }

  // ✏️ EDIT
  editHospital(hospital: any): void {
    this.dialog.open(HospitalDialogComponent, {
      width: '500px',
      data: {
      id: hospital.hospitalProviderId,   // ✅ this is the key line
      providerName: hospital.providerName,
      providerType: hospital.providerType,
      city: hospital.city,
      isNetworkProvider: hospital.isNetworkProvider
    }
    }).afterClosed().subscribe(ok => {
      if (ok) this.loadHospitals();
    });
  }

  // 🔁 NETWORK TOGGLE
  toggleNetwork(h: any): void {
    this.hospitalService
      .toggleNetwork(h.id, !h.isNetworkProvider)
      .subscribe(() => this.loadHospitals());
  }

  // 🔁 ACTIVE TOGGLE
  toggleActive(h: any): void {
    this.hospitalService
      .toggleActive(h.id, !h.isActive)
      .subscribe(() => this.loadHospitals());
  }
}

