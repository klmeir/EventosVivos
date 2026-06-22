import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ReservationService } from '../reservation.service';

@Component({
  selector: 'app-cancel-reservation',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './cancel-reservation.component.html'
})
export class CancelReservationComponent {
  private readonly reservationService = inject(ReservationService);
  private readonly fb = inject(FormBuilder);

  loading = signal(false);
  error = signal<string | null>(null);
  success = signal<boolean>(false);

  form = this.fb.nonNullable.group({
    reservationId: ['', Validators.required]
  });

  onCancel() {
    if (this.form.valid) {
      this.loading.set(true);
      this.error.set(null);
      
      const { reservationId } = this.form.getRawValue();

      this.reservationService.cancelReservation(reservationId).subscribe({
        next: () => {
          this.loading.set(false);
          this.success.set(true);
        },
        error: (err) => {
          this.loading.set(false);
          this.error.set(err.error?.title || 'Error cancelling reservation. Check if ID is correct or already confirmed.');
        }
      });
    }
  }
}