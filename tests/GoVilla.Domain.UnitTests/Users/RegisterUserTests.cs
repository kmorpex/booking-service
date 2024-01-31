using FluentAssertions;
using GoVilla.Domain.Shared.ValueObjects;
using GoVilla.Domain.Users;
using GoVilla.Domain.Users.Events;
using GoVilla.Domain.Users.ValueObjects;

namespace GoVilla.Domain.UnitTests.Users;

public class RegisterUserTests : BaseTest
{
    [Fact]
    public void Create_Should_Raise_User_Created_Domain_Event()
    {
        // Arrange
        var firstName = new FirstName("first");
        var lastName = new LastName("last");
        var email = new Email("test@test.com");

        // Act
        var user = User.Create(firstName, lastName, email);

        // Assert
        var userCreatedDomainEvent = AssertDomainEventWasPublished<UserCreatedDomainEvent>(user);

        userCreatedDomainEvent.UserId.Should().Be(user.Id);
    }
}