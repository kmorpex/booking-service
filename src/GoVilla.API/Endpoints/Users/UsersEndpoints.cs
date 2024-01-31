using GoVilla.Application.Users.GetLoggedInUser;
using GoVilla.Application.Users.LogInUser;
using GoVilla.Application.Users.RegisterUser;
using GoVilla.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GoVilla.API.Endpoints.Users;

public static class UsersEndpoints
{
    public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder builder)
    {
        var routeGroupBuilder = builder.MapGroup("api/users").RequireAuthorization();
        routeGroupBuilder.MapGet("me", GetLoggedInUser);
        routeGroupBuilder.MapPost("register", Register).AllowAnonymous();
        routeGroupBuilder.MapPost("login", LogIn).AllowAnonymous();

        return builder;
    }

    public static async Task<Results<Ok<UserResponse>, BadRequest<Error>>> GetLoggedInUser(ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetLoggedInUserQuery();
        var result = await sender.Send(query, cancellationToken);

        if (result.IsFailure)
            return TypedResults.BadRequest(result.Error);

        return TypedResults.Ok(result.Value);
    }

    public static async Task<Results<Ok<Guid>, BadRequest<Error>>> Register(RegisterUserRequest request, ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(request.Email, request.FirstName, request.LastName, request.Password);
        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return TypedResults.BadRequest(result.Error);

        return TypedResults.Ok(result.Value);
    }

    public static async Task<Results<Ok<AccessTokenResponse>, BadRequest<Error>>> LogIn(LogInUserRequest request,
        ISender sender, CancellationToken cancellationToken)
    {
        var command = new LogInUserCommand(request.Email, request.Password);
        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return TypedResults.BadRequest(result.Error);

        return TypedResults.Ok(result.Value);
    }
}