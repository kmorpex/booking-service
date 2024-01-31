using System.Reflection;
using GoVilla.Application.Abstractions.Messaging;
using GoVilla.Domain.Abstractions;
using GoVilla.Infrastructure;

namespace GoVilla.ArchitectureTests;

public class BaseTest
{
    protected static Assembly ApplicationAssembly => typeof(IBaseCommand).Assembly;
    protected static Assembly DomainAssembly => typeof(IEntity).Assembly;
    protected static Assembly InfrastructureAssembly => typeof(ApplicationDbContext).Assembly;
}