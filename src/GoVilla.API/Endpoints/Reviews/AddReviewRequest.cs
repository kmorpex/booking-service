namespace GoVilla.API.Endpoints.Reviews;

public sealed record AddReviewRequest(Guid BookingId, int Rating, string Comment);