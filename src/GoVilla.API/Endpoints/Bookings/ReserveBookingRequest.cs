namespace GoVilla.API.Endpoints.Bookings;

public sealed record ReserveBookingRequest(Guid ApartmentId, Guid UserId, DateOnly StartDate, DateOnly EndDate);