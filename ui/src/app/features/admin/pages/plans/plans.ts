import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatTableDataSource } from '@angular/material/table';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';

import { AdminPlanService, InsurancePlan } from '../../services/admin-plan.service';
import { PlanDialogComponent } from './plan-dialog';

@Component({
  selector: 'app-insurance-plans',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatTableModule,
    MatPaginatorModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule,
    MatSlideToggleModule
  ],
  templateUrl: './plans.html',
  styleUrls: ['./plans.scss']
})
export class InsurancePlansComponent implements OnInit {

  displayedColumns = [
    'planName',
    'planType',
    'premiumAmount',
    'coverageAmount',
    'durationInMonths',
    'status',
    'actions'
  ];

  dataSource = new MatTableDataSource<InsurancePlan>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(
    private planService: AdminPlanService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadPlans();
  }

  loadPlans(): void {
    this.planService.getPlans().subscribe(plans => {
      this.dataSource.data = plans;
      this.dataSource.paginator = this.paginator;
    });
  }

  applyFilter(event: Event): void {
    const value = (event.target as HTMLInputElement).value;
    this.dataSource.filter = value.trim().toLowerCase();
  }

  addPlan(): void {
    this.dialog.open(PlanDialogComponent, {
      width: '500px'
    }).afterClosed().subscribe(res => {
      if (res) this.loadPlans();
    });
  }

  editPlan(plan: InsurancePlan): void {
    this.dialog.open(PlanDialogComponent, {
      width: '500px',
      data: plan
    }).afterClosed().subscribe(res => {
      if (res) this.loadPlans();
    });
  }

  disablePlan(planId: number): void {
    if (!confirm('Disable this plan?')) return;
    this.planService.disablePlan(planId).subscribe(() => this.loadPlans());
  }

  toggleStatus(plan: InsurancePlan, isActive: boolean): void {

  // optimistic UI update
  plan.isActive = isActive;

  this.planService
    .togglePlanStatus(plan.insurancePlanId, isActive)
    .subscribe({
      next: () => this.loadPlans(),
      error: () => {
        // rollback on failure
        plan.isActive = !isActive;
      }
    });
}
}
