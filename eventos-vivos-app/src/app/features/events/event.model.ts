export interface Event {
  id: string;
  title: string;
  description: string;
  venueId: string;
  maxCapacity: number;
  availableTickets: number;
  startTime: string; // O Date, si prefieres convertirlo
  endTime: string;
  price: number;
  eventType: 'Concert' | 'Conference' | 'Workshop';
  status: 'Active' | 'Cancelled' | 'Finished';
}

export interface EventFilters {
  type?: string;
  startDate?: string;
  venueId?: string;
  status?: string;
  titleSearch?: string;
}

export interface EventReport {
  totalTicketsSold: number;
  totalTicketsAvailable: number;
  occupancyPercentage: number;
  totalRevenue: number;
  status: string;
}