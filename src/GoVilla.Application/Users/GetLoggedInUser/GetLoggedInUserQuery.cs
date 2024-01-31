using GoVilla.Application.Abstractions.Messaging;

namespace GoVilla.Application.Users.GetLoggedInUser;

public sealed record GetLoggedInUserQuery : IQuery<UserResponse>;