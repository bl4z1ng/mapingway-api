using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;
using Mapingway.API;
using Mapingway.Application;
using Mapingway.Domain;
using Mapingway.Infrastructure;
using NetArchTest.Rules;

namespace Architecture.Tests;

public class ArchitectureTests
{
    public static IEnumerable<object[]> GetProjectArchitectureRules()
    {
        var domainAssembly = Domain.AssemblyReference;
        var applicationAssembly = Application.AssemblyReference;
        var infrastructureAssembly = Infrastructure.AssemblyReference;
        var presentationAssembly = Presentation.AssemblyReference;

        var applicationNamespace = applicationAssembly.GetName().Name;
        var infrastructureNamespace = infrastructureAssembly.GetName().Name;
        var presentationNamespace = presentationAssembly.GetName().Name;

        return new List<object[]>
        {
            new object[]{ domainAssembly,
                new[]
                {
                    applicationNamespace,
                    infrastructureNamespace,
                    presentationNamespace
                }},
            new object[]{ applicationAssembly,
                new[]
                {
                    infrastructureNamespace, 
                    presentationNamespace
                }},
            new object[]{ infrastructureAssembly, 
                new[] { presentationNamespace }}
        };
    }

    [Theory]
    [MemberData(nameof(GetProjectArchitectureRules))]
    public void Project_Should_Not_HaveDependenciesOnProjects(Assembly assembly, string[] prohibitedAssemblies)
    {
        var result = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(prohibitedAssemblies)
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            $"{assembly.GetName()} should not depend on: {string.Join(", ", prohibitedAssemblies)}");
    }
}