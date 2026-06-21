import { Component, inject, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  
  loading = signal(false);
  error = signal<string | null>(null);

  loginForm = new FormGroup({
    username: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required])
  });

  onSubmit() {
    if (this.loginForm.valid) {
      this.loading.set(true);
      this.error.set(null);

      const { username, password } = this.loginForm.value;
      
      this.authService.login({ username: username!, password: password! }).subscribe({
        next: () => this.router.navigate(['/']),
        error: (err) => {
          this.loading.set(false);
          this.error.set('Error al iniciar sesión: ' + err.message);
        }
      });
    }
  }
}