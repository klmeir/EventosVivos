import { computed, inject, Service, signal } from '@angular/core';
import { ApiService } from '../../core/services/api.service';
import { LoginRequest, LoginResponse } from './auth.models';
import { tap } from 'rxjs';
import { jwtDecode } from 'jwt-decode';

export interface DecodedToken {
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role': string;
}

@Service()
export class AuthService {
  private readonly api = inject(ApiService);
    
  token = signal<string | null>(localStorage.getItem('access_token'));
  isAuthenticated = signal<boolean>(!!localStorage.getItem('access_token'));
  
  userRole = signal<string | null>(this.getRoleFromToken());

  private getRoleFromToken(): string | null {
    const token = this.token();
    if (!token) return null;
    try {      
      const decoded: DecodedToken = jwtDecode(token);      
      return decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || null;
    } catch {
      return null;
    }
  }

  isAdmin = computed(() => this.userRole() === 'admin');

  login(credentials: LoginRequest) {
    return this.api.post<LoginResponse>('/auth/login', credentials).pipe(
      tap(response => {
        localStorage.setItem('access_token', response.token);
        this.token.set(response.token);
        this.isAuthenticated.set(true);
        this.userRole.set(this.getRoleFromToken());
      })
    );
  }

  logout() {
    this.token.set(null);
    this.isAuthenticated.set(false);
    this.userRole.set(null);
    localStorage.removeItem('access_token');
  }
}