import { Injectable, inject } from '@angular/core';
import { ApiService } from '../../core/services/api.service';
import { HttpParams } from '@angular/common/http';
import { Event, EventFilters, EventReport } from './event.model';

@Injectable({ providedIn: 'root' })
export class EventService {
  private readonly api = inject(ApiService);

  getEvents(filters: EventFilters) {
    let params = new HttpParams();
    
    if (filters.type) params = params.set('type', filters.type);
    if (filters.startDate) params = params.set('startDate', filters.startDate);
    if (filters.venueId) params = params.set('venueId', filters.venueId);
    if (filters.status) params = params.set('status', filters.status);
    if (filters.titleSearch) params = params.set('titleSearch', filters.titleSearch);
    
    return this.api.get<Event[]>('/events', params);
  }

  createEvent(event: Partial<Event>) {
    return this.api.post<Event>('/events', event);
  }

  getEventById(id: string) {
    return this.api.get<Event>(`/events/${id}`);
  }

  getEventReport(id: string) {
    return this.api.get<EventReport>(`/events/${id}/report`);
  }
}