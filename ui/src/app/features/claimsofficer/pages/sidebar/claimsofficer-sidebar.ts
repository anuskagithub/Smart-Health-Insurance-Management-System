import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../../../core/services/auth.service';


@Component({
  selector: 'app-claimsofficer-sidebar',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './claimsofficer-sidebar.html',
  styleUrls: ['./claimsofficer-sidebar.scss']
})
export class ClaimsOfficerSidebarComponent {

  constructor(private authService: AuthService) {}

  logout() {
    this.authService.logout();
  }
}
