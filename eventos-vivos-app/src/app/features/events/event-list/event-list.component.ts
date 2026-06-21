import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { EventService } from '../event.service';
import { DatePipe, NgClass } from '@angular/common';
import { Event } from '../event.model';

@Component({
  selector: 'app-event-list',
  standalone: true,
  imports: [RouterLink, DatePipe, NgClass],
  templateUrl: './event-list.component.html'
})
export class EventListComponent implements OnInit {
  private eventService = inject(EventService);
  
  // Using English naming convention
  events = signal<Event[]>([]);
  isLoading = signal(true);
  errorMessage = signal<string | null>(null);

  ngOnInit() {
    this.loadEvents();
  }

  loadEvents() {
    this.isLoading.set(true);
    this.eventService.getEvents({}).subscribe({
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
}