import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-customer-sidebar',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './customer-sidebar.html',
  styleUrls: ['./customer-sidebar.scss']
})
export class CustomerSidebarComponent {

  constructor(private authService: AuthService) {}

  logout() {
    this.authService.logout();
  }
}
