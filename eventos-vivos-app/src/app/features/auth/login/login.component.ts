import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './login.component.html'
})
export class LoginComponent {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);
  
  loading = signal(false);
  error = signal<string | null>(null);

  loginForm = this.fb.nonNullable.group({
    username: ['', Validators.required],
    password: ['', Validators.required]
  });

  constructor() {
    if (this.authService.isAuthenticated()) {
      this.router.navigate(['/']);
    }
  }

  onSubmit() {
    if (this.loginForm.valid) {
      this.loading.set(true);
      this.error.set(null);

      this.authService.login(this.loginForm.getRawValue()).subscribe({
        next: () => this.router.navigate(['/']),
        error: (err) => {
          this.loading.set(false);
          this.error.set(
            err.status === 401 
            ? 'Invalid username or password.' 
            : 'An unexpected error occurred. Please try again later.'
          );
        }
      });
    }
  }
}