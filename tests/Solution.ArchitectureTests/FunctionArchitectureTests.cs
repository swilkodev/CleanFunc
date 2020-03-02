using System;
using Xunit;
using NetArchTest.Rules;
using System.Reflection;
using System.Text;

namespace Solution.ArchitectureTests
{
    public class FunctionArchitectureTests
    {
        private static Assembly FunctionAppAssembly => typeof(CleanFunc.FunctionApp.Startup).Assembly;
        
        //[Fact]
        public void Controllers_MustHaveNameEndingInController()
        {
            var result = Types.InAssembly(FunctionAppAssembly)
                .That()
                .ResideInNamespace("CleanFunc.FunctionApp")
                .And().AreClasses()
                .And().AreNotAbstract()
                .Should().HaveNameEndingWith("Manager")
                .GetResult();
            
            Assert.True(result.IsSuccessful, GetErrorMessage(result));
        }
        
        //[Fact]
        public void Controllers_MustNotDependOnInfrastructure()
        {
            var result = Types.InAssembly(FunctionAppAssembly)
                .That()
                .ResideInNamespace("CleanFunc.FunctionApp")
                .ShouldNot()
                .HaveDependencyOn("CleanFunc.Infrastructure")
                .GetResult();
            
            Assert.True(result.IsSuccessful, GetErrorMessage(result));
        }
        
        // [Fact]
        // public void Controllers_MustNotDependOnPersistence()
        // {
        //     var result = Types.InAssembly(FunctionAppAssembly)
        //         .That()
        //         .ResideInNamespace("Northwind.WebUI.Controllers")
        //         .ShouldNot()
        //         .HaveDependencyOn("CleanFunc.Persistence")
        //         .GetResult();
            
        //     Assert.True(result.IsSuccessful, GetErrorMessage(result));
        // }
        
        [Fact]
        public void Controllers_MustNotDependOnDomain()
        {
            var result = Types.InAssembly(FunctionAppAssembly)
                .That()
                .ResideInNamespace("CleanFunc.FunctionApp")
                .ShouldNot()
                .HaveDependencyOn("CleanFunc.Domain")
                .GetResult();
            
            Assert.True(result.IsSuccessful, GetErrorMessage(result));
        }

        private static string GetErrorMessage(TestResult result)
        {
            if (result.IsSuccessful)
            {
                return string.Empty;
            }
            
            var builder = new StringBuilder();
            builder.AppendLine("Failing types:");

            foreach (var typeName in result.FailingTypeNames)
            {
                builder.AppendLine($"  - {typeName}");
            }

            return builder.ToString();

        }
    }
}
