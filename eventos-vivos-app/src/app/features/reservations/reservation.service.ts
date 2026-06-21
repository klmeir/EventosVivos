import { inject, Injectable } from '@angular/core';
import { ApiService } from '../../core/services/api.service';
import { Reservation, ReservationCreateDto } from './reservation.model';

@Injectable({ providedIn: 'root' })
export class ReservationService {
  private readonly api = inject(ApiService);

  createReservation(reservation: ReservationCreateDto) {
    return this.api.post<Reservation>('/reservations', reservation);
  }

  getReservationsByEvent(eventId: string) {
    return this.api.get<Reservation[]>(`/reservations/event/${eventId}`);
  }

  confirmReservation(id: string) {
    // Assuming your API expects a POST and you are handling auth via Interceptor
    return this.api.post<void>(`/reservations/${id}/confirm`, {});
  }

  cancelReservation(id: string) {
    return this.api.post<void>(`/reservations/${id}/cancel`, {});
  }
}