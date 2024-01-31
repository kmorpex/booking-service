using GoVilla.Application.Abstractions.Messaging;

namespace GoVilla.Application.Users.LogInUser;

public sealed record LogInUserCommand(string Email, string Password)
    : ICommand<AccessTokenResponse>;