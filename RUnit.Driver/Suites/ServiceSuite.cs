using RUnit.Introspection;
using RUnit.Library.Matchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitUnderTest;

namespace RUnit.Driver.Suites
{
    [RSuite]
    class ServiceSuite
    {
        DependantType dependant;
        RMock<IExampleService> exampleServiceWrapper;
        [RSetup]
        public void Setup()
        {
            exampleServiceWrapper = RMock<IExampleService>.Builder()
                .DefineReturn("GetMessage", "Hello World")
                .DefineReturn("GetPerson", new Person("Rich"))
                .DefineReturn("GetValue", 13)
                .Compile();
            dependant = new DependantType(exampleServiceWrapper.Instance);
        }
        [RTest]
        public void PersonIsRich()
        {
            Matchers.Same(dependant.GetPerson(), new Person("Rich"));
        }
        [RTest]
        public void MessageSaysHelloWorld()
        {
            Matchers.Same(dependant.GetMessage(), "Hello World");
        }
        [RTest]
        public void IsValueGreaterThanTwelve()
        {
            Matchers.GreaterThan(dependant.GetValue(), 12);
        }
    }
}
