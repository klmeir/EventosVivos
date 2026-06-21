export interface ReservationCreateDto {
  eventId: string;
  quantity: number;
  buyerName: string;
  buyerEmail: string;
}
export interface Reservation extends ReservationCreateDto {
  id: string;
  status: 'PendingPayment' | 'Confirmed' | 'Cancelled';
  reservationCode: string;
  createdAt: string;
}