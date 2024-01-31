using FluentAssertions;
using GoVilla.Domain.Abstractions;
using NetArchTest.Rules;

namespace GoVilla.ArchitectureTests.Domain;

public class DomainTests : BaseTest
{
    [Fact]
    public void Domain_Event_Should_Have_DomainEvent_Postfix()
    {
        var result = Types.InAssembly(DomainAssembly)
            .That()
            .ImplementInterface(typeof(IDomainEvent))
            .Should().HaveNameEndingWith("DomainEvent")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}