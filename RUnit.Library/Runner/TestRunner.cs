using RUnit.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUnit.Runner
{
    public static class TestRunner
    {
        public static void Run(Assembly[] assembly)
        {
            var testSuites = assembly.SelectMany(a => a.GetTypes().Where(t => t.GetCustomAttributes<RSuiteAttribute>().Count() > 0)).ToArray();
            Console.WriteLine($"Test suites found {testSuites.Count()}");
            Console.WriteLine();
            RunSuites(testSuites);
        }
        private static void RunSuites(Type[] suites)
        {
            foreach (var suite in suites)
            {
                Console.WriteLine($"- {suite.Name}");
                var setupMethods = GetMethods<RSetupAttribute>(suite);
                var testMethods = GetMethods<RTestAttribute>(suite);
                var teardownMethods = GetMethods<RTeardownAttribute>(suite);

                var suiteInstance = Activator.CreateInstance(suite);

                RunSetupMethods(setupMethods, suiteInstance);

                RunTests(testMethods, suiteInstance);
                Console.WriteLine();
            }
        }
        private static MethodInfo[] GetMethods<T>(Type suite) where T : System.Attribute
        {
            var methods = suite.GetMethods().Where(m => m.GetCustomAttributes<T>().Count() > 0);
            return methods.ToArray();
        }
        private static void RunSetupMethods<T>(MethodInfo[] methods, T runningSuite)
        {
            foreach (var method in methods)
            {
                method.Invoke(runningSuite, new object[] { });
            }
        }
        private static void RunTests<T>(MethodInfo[] methods, T runningSuite)
        {
            foreach (var test in methods)
            {
                var testAttribute = test.GetCustomAttribute<RTestAttribute>();
                var expectsException = testAttribute.ExpectsException;
                try
                {
                    test.Invoke(runningSuite, new object[] { });
                }
                catch (Exception e)
                {
                    if (expectsException)
                    {
                        var exceptionType = testAttribute.ExceptionType;
                        if(e.InnerException.GetType() == exceptionType)
                        {
                            Console.WriteLine($"|  {test.Name} - PASSED");
                        }
                    } 
                    else if(e.InnerException.GetType() == typeof(MatchedSuccessException))
                    {                        
                        Console.WriteLine($"|  {test.Name} - PASSED");
                    }
                    else
                    {
                        Console.WriteLine($"|* {test.Name} - FAILED");
                    }
                }
            }
        }
    }
}
