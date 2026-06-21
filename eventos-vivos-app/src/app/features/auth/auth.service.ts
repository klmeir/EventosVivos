import { inject, Service, signal } from '@angular/core';
import { ApiService } from '../../core/services/api.service';
import { LoginRequest, LoginResponse } from './auth.models';
import { tap } from 'rxjs';

@Service()
export class AuthService {
  private readonly api = inject(ApiService);
  
  // Estado reactivo: accesible desde cualquier componente
  token = signal<string | null>(localStorage.getItem('access_token'));
  isAuthenticated = signal<boolean>(!!localStorage.getItem('access_token'));

  login(credentials: LoginRequest) {
    return this.api.post<LoginResponse>('/auth/login', credentials).pipe(
      tap(response => {
        this.token.set(response.token);
        this.isAuthenticated.set(true);
        localStorage.setItem('access_token', response.token);
      })
    );
  }

  logout() {
    this.token.set(null);
    this.isAuthenticated.set(false);
    localStorage.removeItem('access_token');
  }
}