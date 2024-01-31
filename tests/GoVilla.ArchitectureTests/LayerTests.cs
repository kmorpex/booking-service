using FluentAssertions;
using NetArchTest.Rules;

namespace GoVilla.ArchitectureTests;

public class LayerTests : BaseTest
{
    [Fact]
    public void Domain_Layer_Should_Not_Have_Dependency_On_Application_Layer()
    {
        var result = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(ApplicationAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Domain_Layer_Should_Not_Have_Dependency_On_Infrastructure_Layer()
    {
        var result = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(ApplicationAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Application_Layer_Should_Not_Have_Dependency_On_Infrastructure_Layer()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}