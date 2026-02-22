namespace RestaurantManagement.Api.ArchTests;

using NetArchTest.Rules;
using NUnit.Framework;
using System.Reflection;

using FluentValidation;

using Mediator;

[TestFixture]
public class DesignPatternTests
{
    private const string RestaurantApiAssembly = "RestaurantManagement.Api";
    private const string FeaturesNamespace = "RestaurantManagement.Api.Features";

    [Test]
    public void Handlers_ShouldImplementIRequestHandler()
    {
        var result = Types.InAssembly(Assembly.Load(RestaurantApiAssembly))
            .That()
            .HaveNameEndingWith("Handler")
            .And()
            .ResideInNamespace(FeaturesNamespace)
            .Should()
            .ImplementInterface(typeof(IRequestHandler<,>))
            .GetResult();

        Assert.That(result.IsSuccessful, 
            $"All handlers should implement IRequestHandler<,>. " +
            $"Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Test]
    public void Handlers_ShouldBeSealed()
    {
        var result = Types.InAssembly(Assembly.Load(RestaurantApiAssembly))
            .That()
            .HaveNameEndingWith("Handler")
            .And()
            .ResideInNamespace(FeaturesNamespace)
            .Should()
            .BeSealed()
            .GetResult();

        Assert.That(result.IsSuccessful, 
            $"All handlers should be sealed classes. " +
            $"Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Test]
    public void CommandAndQueries_ShouldBeSealed()
    {
        var result = Types.InAssembly(Assembly.Load(RestaurantApiAssembly))
            .That()
            .ImplementInterface(typeof(IRequest<>))
            .And()
            .ResideInNamespace(FeaturesNamespace)
            .Should()
            .BeSealed()
            .GetResult();

        Assert.That(result.IsSuccessful,
            $"All command and queries should be sealed classes. " +
            $"Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Test]
    public void Validators_ShouldBeSealed()
    {
        var result = Types.InAssembly(Assembly.Load(RestaurantApiAssembly))
            .That()
            .Inherit(typeof(AbstractValidator<>))
            .And()
            .ResideInNamespace(FeaturesNamespace)
            .Should()
            .BeSealed()
            .GetResult();

        Assert.That(result.IsSuccessful,
            $"All validators should be sealed classes. " +
            $"Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Test]
    public void Queries_ShouldImplementIRequest()
    {
        var result = Types.InAssembly(Assembly.Load(RestaurantApiAssembly))
            .That()
            .HaveNameEndingWith("Query")
            .And()
            .ResideInNamespace(FeaturesNamespace)
            .Should()
            .ImplementInterface(typeof(IRequest<>))
            .GetResult();

        Assert.That(result.IsSuccessful, 
            $"All queries should implement IRequest<>. " +
            $"Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Test]
    public void Commands_ShouldImplementIRequest()
    {
        var result = Types.InAssembly(Assembly.Load(RestaurantApiAssembly))
            .That()
            .HaveNameEndingWith("Command")
            .And()
            .ResideInNamespace(FeaturesNamespace)
            .Should()
            .ImplementInterface(typeof(IRequest<>))
            .GetResult();

        Assert.That(result.IsSuccessful, 
            $"All commands should implement IRequest<>. " +
            $"Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Test]
    public void Queries_ShouldBeRecords()
    {
        var assembly = Assembly.Load(RestaurantApiAssembly);
        var queryTypes = Types.InAssembly(assembly)
            .That()
            .HaveNameEndingWith("Query")
            .And()
            .ResideInNamespace(FeaturesNamespace)
            .GetTypes();

        var nonRecordTypes = queryTypes
            .Where(type => !IsRecord(type))
            .ToList();

        Assert.That(nonRecordTypes, Is.Empty, 
            $"All queries should be records. Non-record types: {string.Join(", ", nonRecordTypes.Select(t => t.Name))}");
    }

    [Test]
    public void Commands_ShouldBeRecords()
    {
        var assembly = Assembly.Load(RestaurantApiAssembly);
        var commandTypes = Types.InAssembly(assembly)
            .That()
            .HaveNameEndingWith("Command")
            .And()
            .ResideInNamespace(FeaturesNamespace)
            .GetTypes();

        var nonRecordTypes = commandTypes
            .Where(type => !IsRecord(type))
            .ToList();

        Assert.That(nonRecordTypes, Is.Empty, 
            $"All commands should be records. Non-record types: {string.Join(", ", nonRecordTypes.Select(t => t.Name))}");
    }

    [Test]
    public void Validators_ShouldInheritFromAbstractValidator()
    {
        var result = Types.InAssembly(Assembly.Load(RestaurantApiAssembly))
            .That()
            .HaveNameEndingWith("Validator")
            .And()
            .ResideInNamespace(FeaturesNamespace)
            .Should()
            .Inherit(typeof(FluentValidation.AbstractValidator<>))
            .GetResult();

        Assert.That(result.IsSuccessful, 
            $"All validators should inherit from AbstractValidator<>. " +
            $"Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Test]
    public void Endpoints_ShouldBeStaticClasses()
    {
        var result = Types.InAssembly(Assembly.Load(RestaurantApiAssembly))
            .That()
            .HaveNameEndingWith("Endpoint")
            .And()
            .ResideInNamespace(FeaturesNamespace)
            .Should()
            .BeStatic()
            .GetResult();

        Assert.That(result.IsSuccessful, 
            $"All endpoints should be static classes. " +
            $"Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Test]
    public void Responses_ShouldBeRecords()
    {
        var assembly = Assembly.Load(RestaurantApiAssembly);
        var responseTypes = Types.InAssembly(assembly)
            .That()
            .HaveNameEndingWith("Response")
            .And()
            .ResideInNamespace(FeaturesNamespace)
            .GetTypes();

        var nonRecordTypes = responseTypes
            .Where(type => !IsRecord(type))
            .ToList();

        Assert.That(nonRecordTypes, Is.Empty, 
            $"All responses should be records. Non-record types: {string.Join(", ", nonRecordTypes.Select(t => t.Name))}");
    }

    [Test]
    public void DTOs_ShouldBeRecords()
    {
        var assembly = Assembly.Load(RestaurantApiAssembly);
        var dtoTypes = Types.InAssembly(assembly)
            .That()
            .HaveNameEndingWith("Dto")
            .And()
            .ResideInNamespace(FeaturesNamespace)
            .GetTypes();

        var nonRecordTypes = dtoTypes
            .Where(type => !IsRecord(type))
            .ToList();

        Assert.That(nonRecordTypes, Is.Empty, 
            $"All DTOs should be records. Non-record types: {string.Join(", ", nonRecordTypes.Select(t => t.Name))}");
    }

    private static bool IsRecord(Type type)
    {
        // A record type has a protected copy constructor and inherits from IEquatable<T>
        return type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                   .Any(m => m.Name == "<Clone>$" || 
                            (m.Name.Equals(".ctor") && 
                             m.GetParameters().Length == 1 && 
                             m.GetParameters()[0].ParameterType == type)) ||
               type.GetInterfaces().Any(i => i.IsGenericType && 
                                            i.GetGenericTypeDefinition() == typeof(IEquatable<>) &&
                                            i.GetGenericArguments()[0] == type);
    }
}