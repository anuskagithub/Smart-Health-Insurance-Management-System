import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ClaimsOfficerSidebarComponent } from '../sidebar/claimsofficer-sidebar';


@Component({
  selector: 'app-claimsofficer-layout',
  standalone: true,
  imports: [RouterOutlet, ClaimsOfficerSidebarComponent],
  templateUrl: './claimsofficer-layout.html',
  styleUrls: ['./claimsofficer-layout.scss']
})
export class ClaimsOfficerLayoutComponent {}
