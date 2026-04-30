using CleanStarter.API.Controllers.V1;
using CleanStarter.Application.Features.Auth.Commands.Login;
using CleanStarter.Core.Entities;
using CleanStarter.Infrastructure.Data;
using FluentAssertions;
using MediatR;
using NetArchTest.Rules;
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
           
            var result = Types.InAssembly(typeof(LoginCommand).Assembly)
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
        public void Controllers_Should_Not_Depend_Directly_On_Infrastructure()
        {
          
            var result = Types.InAssembly(typeof(AuthController).Assembly)
                .That()
                .HaveNameEndingWith("Controller")
                .ShouldNot()
                .HaveDependencyOn(InfrastructureNamespace)
                .GetResult();

            result.IsSuccessful.Should().BeTrue("Controllers should communicate via MediatR ISender, not Infrastructure directly.");
        }

        [Fact]
        public void Handlers_Should_Have_Name_Ending_With_Handler()
        {
            var result = Types.InAssembly(typeof(LoginCommand).Assembly)
                .That()
                .ImplementInterface(typeof(IRequestHandler<,>))
                .Should()
                .HaveNameEndingWith("Handler")
                .GetResult();

            result.IsSuccessful.Should().BeTrue("All MediatR handlers must have a name ending with 'Handler'.");
        }

        [Fact]
        public void Commands_Should_Have_Name_Ending_With_Command()
        {
           
            var result = Types.InAssembly(typeof(LoginCommand).Assembly)
                .That()
                .ResideInNamespaceContaining("Commands")
                .And()
                .AreNotInterfaces()
                .And()
                .DoNotHaveNameEndingWith("Validator")
                .And()
                .DoNotHaveNameEndingWith("Handler")
                .Should()
                .HaveNameEndingWith("Command")
                .GetResult();

            result.IsSuccessful.Should().BeTrue("All request objects in Commands namespaces must end with 'Command'.");
        }

        [Fact]
        public void Queries_Should_Have_Name_Ending_With_Query()
        {
    
            var result = Types.InAssembly(typeof(LoginCommand).Assembly)
                .That()
                .ResideInNamespaceContaining("Queries")
                .And()
                .AreNotInterfaces()
                .And()
                .DoNotHaveNameEndingWith("Validator")
                .And()
                .DoNotHaveNameEndingWith("Handler")
                .Should()
                .HaveNameEndingWith("Query")
                .GetResult();

            result.IsSuccessful.Should().BeTrue("All request objects in Queries namespaces must end with 'Query'.");
        }
    }
}