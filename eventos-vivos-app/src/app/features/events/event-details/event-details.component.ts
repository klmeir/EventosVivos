import { Component, inject, signal, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { DatePipe, NgClass } from '@angular/common';
import { EventService } from '../event.service';
import { Event } from '../event.model';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { ReservationService } from '../../reservations/reservation.service';
import { Reservation, ReservationCreateDto } from '../../reservations/reservation.model';
import { AuthService } from '../../auth/auth.service';

@Component({
  selector: 'app-event-details',
  standalone: true,
  imports: [RouterLink, DatePipe, ReactiveFormsModule, NgClass],
  templateUrl: './event-details.component.html'
})
export class EventDetailsComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private authService = inject(AuthService);
  private eventService = inject(EventService);
  private resService = inject(ReservationService);
  private fb = inject(FormBuilder);
  
  event = signal<Event | null>(null);
  reservations = signal<Reservation[]>([]);
  isLoading = signal(true);
  isAdmin = this.authService.isAdmin;
  reservationResult = signal<{ id: string } | null>(null);

  reservationForm = this.fb.group({
    quantity: [1, [Validators.required, Validators.min(1)]],
    buyerName: ['', Validators.required],
    buyerEmail: ['', [Validators.required, Validators.email]]
  });

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadEventData(id);
    }
  }

  loadEventData(id: string) {
    this.isLoading.set(true);
    // Fetch Event details
    this.eventService.getEventById(id).subscribe({
      next: (data) => {
        this.event.set(data);
        this.isLoading.set(false);
      }
    });
    // Fetch existing reservations
    this.resService.getReservationsByEvent(id).subscribe({
      next: (data) => this.reservations.set(data)
    });
  }

  submitReservation() {
    if (this.reservationForm.valid && this.event()) {
      const newReservation: ReservationCreateDto = {
        eventId: this.event()!.id,
        quantity: this.reservationForm.value.quantity!,
        buyerName: this.reservationForm.value.buyerName!,
        buyerEmail: this.reservationForm.value.buyerEmail!
      };

      this.resService.createReservation(newReservation).subscribe({
        next: (response: any) => {
          alert('Reservation confirmed successfully!');
          
          this.reservationResult.set({ id: response });

          this.reservationForm.reset({ quantity: 1 });
          // Refresh reservations list
          this.loadEventData(this.event()!.id);
        },
        error: (err) => console.error('Error creating reservation', err)
      });
    }
  }

  confirmPayment(id: string) {
    if (confirm('Confirm this payment?')) {
      this.resService.confirmReservation(id).subscribe(() => {
        alert('Payment confirmed!');
        this.loadEventData(this.event()!.id); // Refresh data
      });
    }
  }

  cancelReservation(id: string) {
    if (confirm('Are you sure you want to cancel this reservation?')) {
      this.resService.cancelReservation(id).subscribe(() => {
        alert('Reservation cancelled.');
        this.loadEventData(this.event()!.id); // Refresh data
      });
    }
  }

  copyToClipboard(text: string) {
    navigator.clipboard.writeText(text).then(() => {
      alert('ID copied to the clipboard');
    });
  }
}