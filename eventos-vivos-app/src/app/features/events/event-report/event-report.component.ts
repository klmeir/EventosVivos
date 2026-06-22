import { Component, inject, signal, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { EventService } from '../event.service';
import { DecimalPipe } from '@angular/common';

interface EventReport {
  totalTicketsSold: number;
  totalTicketsAvailable: number;
  occupancyPercentage: number;
  totalRevenue: number;
  status: string;
}

@Component({
  selector: 'app-event-report',
  standalone: true,
  imports: [RouterLink, DecimalPipe],
  templateUrl: './event-report.component.html'
})
export class EventReportComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private eventService = inject(EventService);
  
  report = signal<EventReport | null>(null);

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.eventService.getEventReport(id).subscribe(data => this.report.set(data));
    }
  }
}