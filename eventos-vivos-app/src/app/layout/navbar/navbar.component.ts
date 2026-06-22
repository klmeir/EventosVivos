import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../features/auth/auth.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  template: `
    <nav class="navbar navbar-expand-lg navbar-dark bg-primary mb-4">
      <div class="container">
        <a class="navbar-brand" href="#">Eventos Vivos</a>
        <div class="collapse navbar-collapse">
          <ul class="navbar-nav ms-auto">
            <li class="nav-item">
              <a class="nav-link" routerLink="/login" routerLinkActive="active">Admin Login</a>
            </li>
            <li class="nav-item">
              <a class="nav-link" (click)="logout()" style="cursor: pointer;">Go out</a>
            </li>
          </ul>
        </div>
      </div>
    </nav>
  `
})
export class NavbarComponent {
  public authService = inject(AuthService);
  private router = inject(Router);
  
  logout() {    
    this.authService.logout();
    this.router.navigate(['/events']);
  }
}