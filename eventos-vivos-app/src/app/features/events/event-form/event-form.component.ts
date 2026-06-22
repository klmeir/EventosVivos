import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { EventService } from '../event.service';
import { NgClass } from '@angular/common';
import { VenueService } from '../venue.service';
import { Venue } from '../venue.model';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-event-form',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink, NgClass],
  templateUrl: './event-form.component.html'
})
export class EventFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private eventService = inject(EventService);
  private venueService = inject(VenueService);
  private router = inject(Router);

  venues = signal<Venue[]>([]);
  isSubmitting = signal(false);
  errorMessage = signal<string | null>(null);

  ngOnInit() {
    this.loadVenues();
  }

  loadVenues() {
    this.venueService.getVenues().subscribe({
      next: (data) => this.venues.set(data),
      error: () => console.error('Failed to load venues')
    });
  }

  // Form definition
  eventForm = this.fb.group({
    title: ['', [Validators.required, Validators.minLength(5)]],
    description: ['', [Validators.required, Validators.minLength(10)]],
    venueId: ['', [Validators.required]],
    maxCapacity: [10, [Validators.required, Validators.min(1)]],
    startTime: ['', [Validators.required]],
    endTime: ['', [Validators.required]],
    price: [0, [Validators.required, Validators.min(0)]],
    eventType: ['Concert', [Validators.required]],
    status: ['Active']
  });

  onSubmit() {
    this.errorMessage.set(null);
    if (this.eventForm.valid) {
      this.isSubmitting.set(true);
      
      this.eventService.createEvent(this.eventForm.value as any).subscribe({
        next: () => {
          this.router.navigate(['/events']);
        },
        error: (err: HttpErrorResponse) => {          
          if (err.status === 400 && err.error.errors) {            
            const allErrors = Object.values(err.error.errors)
                                    .map((e: any) => e[0])
                                    .join(' | ');

            console.log(allErrors);
            this.errorMessage.set(`Validation Errors: ${allErrors}`);
          }           
          else {
            this.errorMessage.set(err.error?.title || 'An unexpected error occurred');
          }
          this.isSubmitting.set(false);
        }
      });
    } else {
      this.eventForm.markAllAsTouched();
    }
  }
}