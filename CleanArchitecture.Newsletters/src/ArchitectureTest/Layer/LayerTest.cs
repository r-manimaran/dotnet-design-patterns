using NetArchTest.Rules;
using Shouldly;

namespace ArchitectureTest.Layer;

public class LayerTest : BaseTest
{
    [Fact]
    public void Domain_Should_Not_Depend_On_ApplicationLayer()
    {
        TestResult result = Types.InAssembly(DomainAssembly)
                            .ShouldNot()
                            .HaveDependencyOn(ApplicationAssembly.GetName().Name)
                            .GetResult();
        result.IsSuccessful.ShouldBeTrue();
    }

    [Fact]
    public void Domain_Should_Not_Depend_On_Any_Layer()
    {
        List<string> Assemblies = [
            ApplicationAssembly.GetName().Name,
            InfrastrucureAssembly.GetName().Name,
            PresentationAssembly.GetName().Name
            ];

        foreach(var assembly in Assemblies)
        {
            TestResult result = Types.InAssembly(DomainAssembly)
                                     .ShouldNot()
                                     .HaveDependencyOn(assembly)
                                     .GetResult();

            result.IsSuccessful.ShouldBeTrue();
        }
    }

    [Fact]
    public void Application_Should_Not_Depend_On_Any_Except_Domain()
    {
        List<string> Assemblies = [
                                    InfrastrucureAssembly.GetName().Name,
                                    PresentationAssembly.GetName().Name
                                  ];

        foreach(var assembly in Assemblies)
        {
            TestResult result = Types.InAssembly(ApplicationAssembly)
                                .ShouldNot()
                                .HaveDependencyOn(assembly)
                                .GetResult();
            result.IsSuccessful.ShouldBeTrue();

        }

    }
    [Fact]
    public void Infrastructure_Should_not_Depend_On_Presentation()
    {
        TestResult result = Types.InAssembly(InfrastrucureAssembly)
                                .ShouldNot()
                                .HaveDependencyOn(PresentationAssembly.GetName().Name)
                                .GetResult();
        result.IsSuccessful.ShouldBeTrue();
    }
}
