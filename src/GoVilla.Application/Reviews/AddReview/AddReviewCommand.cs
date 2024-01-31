using GoVilla.Application.Abstractions.Messaging;

namespace GoVilla.Application.Reviews.AddReview;

public sealed record AddReviewCommand(Guid BookingId, int Rating, string Comment) : ICommand;