using System;
using Xunit;
using NetArchTest.Rules;
using System.Reflection;
using System.Text;
using CleanFunc.FunctionApp.Base;

namespace Solution.ArchitectureTests
{
    public class FunctionArchitectureTests
    {
        private static Assembly FunctionAppAssembly => typeof(CleanFunc.FunctionApp.Startup).Assembly;
        
        [Fact]
        public void FunctionApp_MustInheritFromBaseClass_And_HaveNameEndingInFunctions()
        {
            var result = Types.InAssembly(FunctionAppAssembly)
                .That()
                .ResideInNamespace("CleanFunc.FunctionApp")
                .And().AreClasses()
                .And().AreNotAbstract()
                .And().Inherit(typeof(HttpFunctionBase))
                .Or().Inherit(typeof(ServiceBusFunctionBase))
                .Should().HaveNameEndingWith("Functions")
                .GetResult();
            
            Assert.True(result.IsSuccessful, GetErrorMessage(result));
        }
        
        [Fact]
        public void FunctionApp_TypesOtherThanStartup_MustNotDependOnInfrastructure()
        {
            var result = Types.InAssembly(FunctionAppAssembly)
                .That()
                .ResideInNamespace("CleanFunc.FunctionApp")
                .And().DoNotHaveName("Startup")
                .ShouldNot()
                .HaveDependencyOn("CleanFunc.Infrastructure")
                .GetResult();
            
            Assert.True(result.IsSuccessful, GetErrorMessage(result));
        }
        
        [Fact]
        public void FunctionApp_MustNotDependOnDomain()
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
