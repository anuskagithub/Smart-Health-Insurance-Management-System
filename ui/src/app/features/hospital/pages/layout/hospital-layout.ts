import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HospitalSidebarComponent } from '../sidebar/hospital-sidebar';


@Component({
  selector: 'app-hospital-layout',
  standalone: true,
  imports: [RouterOutlet, HospitalSidebarComponent],
  templateUrl: './hospital-layout.html',
  styleUrls: ['./hospital-layout.scss']
})
export class HospitalLayoutComponent {}
