using GoVilla.Application.Bookings.GetBooking;
using GoVilla.Application.Bookings.ReserveBooking;
using GoVilla.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GoVilla.API.Endpoints.Bookings;

public static class BookingsEndpoints
{
    public static IEndpointRouteBuilder MapBookingsEndpoints(this IEndpointRouteBuilder builder)
    {
        var routeGroupBuilder = builder.MapGroup("api/bookings").RequireAuthorization();
        routeGroupBuilder.MapGet("{id}", GetBooking).WithName(nameof(GetBooking));
        routeGroupBuilder.MapPost("", ReserveBooking);

        return builder;
    }

    public static async Task<Results<Ok<BookingResponse>, NotFound>> GetBooking(Guid id, ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetBookingQuery(id);
        var result = await sender.Send(query, cancellationToken);

        return result.IsSuccess ? TypedResults.Ok(result.Value) : TypedResults.NotFound();
    }

    public static async Task<Results<CreatedAtRoute<Guid>, BadRequest<Error>>> ReserveBooking(
        ReserveBookingRequest request, ISender sender, CancellationToken cancellationToken)
    {
        // We don't want to expose the command, because we'd be coupling it to the endpoint. We'd be leaking that information, which shouldn't happen.
        var command =
            new ReserveBookingCommand(request.ApartmentId, request.UserId, request.StartDate, request.EndDate);
        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return TypedResults.BadRequest(result.Error);

        // RESTful API convention and the response is going to contain a location header with the route to the get booking endpoint and the id of the newly created booking
        return TypedResults.CreatedAtRoute(result.Value, nameof(GetBooking), new { id = result.Value });
    }
}