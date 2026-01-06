import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CustomerSidebarComponent } from '../sidebar/customer-sidebar';

@Component({
  selector: 'app-customer-layout',
  standalone: true,
  imports: [RouterOutlet, CustomerSidebarComponent],
  templateUrl: './customer-layout.html',
  styleUrls: ['./customer-layout.scss']
})
export class CustomerLayoutComponent {}
