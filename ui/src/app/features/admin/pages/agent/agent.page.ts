import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';

import { AgentService } from './agent.service';
import { AgentDialogComponent } from './agent.dialog';

@Component({
  standalone: true,
  templateUrl: './agent.page.html',
  imports: [
    CommonModule,
    MatTableModule,
    MatPaginatorModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule
  ]
})
export class AgentPage implements OnInit {

  displayedColumns = [   
    'agentCode',
    'fullName', 
    'yearsOfExperience',
    'region',
    'isActive',
    'actions'
  ];

  dataSource = new MatTableDataSource<any>([]);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(
    private service: AgentService,
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

  addAgent() {
    this.dialog.open(AgentDialogComponent)
      .afterClosed()
      .subscribe(ok => ok && this.load());
  }

  editAgent(agent: any) {
    this.dialog.open(AgentDialogComponent, {
      data: {
        
        id: agent.agentProfileId,
        fullName: agent.fullName,
        email: agent.email,
        phoneNumber: agent.phoneNumber,
        address: agent.address,
        dateOfBirth: agent.dateOfBirth,
        agentCode: agent.agentCode,
        yearsOfExperience: agent.yearsOfExperience,
        region: agent.region,
        isActive: agent.isActive
      }
    })
    .afterClosed()
    .subscribe(ok => ok && this.load());
  }
}
