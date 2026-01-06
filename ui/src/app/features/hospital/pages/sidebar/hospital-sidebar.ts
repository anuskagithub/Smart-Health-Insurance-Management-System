import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../../../core/services/auth.service';


@Component({
  selector: 'app-hospital-sidebar',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './hospital-sidebar.html',
  styleUrls: ['./hospital-sidebar.scss']
})
export class HospitalSidebarComponent {

  constructor(private authService: AuthService) {}

  logout() {
    this.authService.logout();
  }
}
