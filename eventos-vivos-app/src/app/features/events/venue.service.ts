import { inject, Injectable } from '@angular/core';
import { ApiService } from '../../core/services/api.service';
import { Venue } from './venue.model';

@Injectable({ providedIn: 'root' })
export class VenueService {
  private readonly api = inject(ApiService);

  getVenues() {
    return this.api.get<Venue[]>('/venues');
  }
}