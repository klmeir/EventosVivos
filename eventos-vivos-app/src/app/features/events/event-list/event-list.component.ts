import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { EventService } from '../event.service';
import { VenueService } from '../venue.service';
import { DatePipe, NgClass } from '@angular/common';
import { Event } from '../event.model';
import { Venue } from '../venue.model';
import { AuthService } from '../../auth/auth.service';

@Component({
  selector: 'app-event-list',
  standalone: true,
  imports: [RouterLink, DatePipe, NgClass, ReactiveFormsModule],
  templateUrl: './event-list.component.html'
})
export class EventListComponent implements OnInit {
  private authService = inject(AuthService);
  private eventService = inject(EventService);
  private venueService = inject(VenueService);
  private fb = inject(FormBuilder);

  events = signal<Event[]>([]);
  venues = signal<Venue[]>([]);
  isLoading = signal(true);
  isAdmin = this.authService.isAdmin;
  errorMessage = signal<string | null>(null);

  filterForm = this.fb.group({
    type: [''],
    startDate: [''],
    venueId: [''],
    status: [''],
    titleSearch: ['']
  });

  ngOnInit() {
    this.loadEvents();
    this.loadVenues();
  }

  loadEvents() {
    this.isLoading.set(true);    
    const filters = this.filterForm.value; 
    
    this.eventService.getEvents(filters as any).subscribe({
      next: (data) => {
        this.events.set(data);
        this.isLoading.set(false);
      },
      error: () => {
        this.errorMessage.set('Failed to load events.');
        this.isLoading.set(false);
      }
    });
  }

   loadVenues() {
    this.venueService.getVenues().subscribe({
      next: (data) => this.venues.set(data),
      error: () => console.error('Failed to load venues')
    });
  }
}
