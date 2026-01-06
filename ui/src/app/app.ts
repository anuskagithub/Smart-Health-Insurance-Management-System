import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { HeaderComponent } from './layout/header/header';
import { FooterComponent } from './layout/footer/footer';

@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app.html',
  styleUrls: ['./app.scss'],
  imports: [
    RouterOutlet,
    HeaderComponent,
    FooterComponent
  ],
   template: `
    <app-header></app-header>

    <main class="app-content">
      <router-outlet></router-outlet>
    </main>

    <app-footer></app-footer>
  `
})
export class App {}
