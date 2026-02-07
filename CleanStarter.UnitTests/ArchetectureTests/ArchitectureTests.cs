using CleanStarter.API.Controllers.V1;
using CleanStarter.Application.Services;
using CleanStarter.Core.Entites;
using CleanStarter.Infrastructure.Data;
using FluentAssertions;
using NetArchTest.Rules;
using System;
using Xunit;

namespace CleanStarter.UnitTests.Architecture
{
    public class ArchitectureTests
    {
        private const string DomainNamespace = "CleanStarter.Core";
        private const string ApplicationNamespace = "CleanStarter.Application";
        private const string InfrastructureNamespace = "CleanStarter.Infrastructure";
        private const string APINamespace = "CleanStarter.API";

        [Fact]
        public void Domain_Should_Not_DependOn_Other_Layers()
        {
            var result = Types.InAssembly(typeof(ApplicationUser).Assembly)
                .ShouldNot()
                .HaveDependencyOn(ApplicationNamespace)
                .And()
                .HaveDependencyOn(InfrastructureNamespace)
                .And()
                .HaveDependencyOn(APINamespace)
                .GetResult();

            result.IsSuccessful.Should().BeTrue("Domain layer should not depend on any other layer.");
        }

        [Fact]
        public void Application_Should_Not_DependOn_Infrastructure_Or_API()
        {
   
            var result = Types.InAssembly(typeof(AuthService).Assembly)
                .ShouldNot()
                .HaveDependencyOn(InfrastructureNamespace)
                .And()
                .HaveDependencyOn(APINamespace)
                .GetResult();

            result.IsSuccessful.Should().BeTrue("Application layer should not depend on Infrastructure or API.");
        }

        [Fact]
        public void Infrastructure_Should_Not_DependOn_API()
        {
            var result = Types.InAssembly(typeof(AppDbContext).Assembly)
                .ShouldNot()
                .HaveDependencyOn(APINamespace)
                .GetResult();

            result.IsSuccessful.Should().BeTrue("Infrastructure layer should not depend on API.");
        }

        [Fact]
        public void Controllers_Should_Not_Depend_Directly_On_Repositories()
        {

            var result = Types.InAssembly(typeof(AuthController).Assembly)
                .That()
                .HaveNameEndingWith("Controller")
                .ShouldNot()
                .HaveDependencyOn("CleanStarter.Infrastructure.Repositories")
                .GetResult();

            result.IsSuccessful.Should().BeTrue("Controllers should communicate via Services/Handlers, not Repositories directly.");
        }

        [Fact]
        public void Services_Should_Have_Name_Ending_With_Service()
        {
           
            var result = Types.InAssembly(typeof(AuthService).Assembly)
                .That()
                .ImplementInterface(typeof(CleanStarter.Application.Interfaces.IAuthService)) // مثال
                .Should()
                .HaveNameEndingWith("Service")
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }
    }
}