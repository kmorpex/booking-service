namespace GoVilla.Application.Abstractions.Email;

public interface IEmailService
{
    Task SendAsync(Domain.Shared.ValueObjects.Email recipient, string subject, string body);
}