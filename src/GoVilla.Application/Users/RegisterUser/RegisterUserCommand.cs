using GoVilla.Application.Abstractions.Messaging;

namespace GoVilla.Application.Users.RegisterUser;

public sealed record RegisterUserCommand(
    string Email,
    string FirstName,
    string LastName,
    string Password) : ICommand<Guid>;