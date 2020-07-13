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
    class CalculatorSuite
    {
        Calculator calculator;

        [RSetup]
        public void Setup()
        {
            calculator = new Calculator();
        }

        [RTest]
        public void Add()
        {
            int a = 4;
            int b = 4;
            int result = 8;
            Matchers.Same(calculator.Add(a, b), result);
        }

        [RTest(ExpectedException:typeof(DivideByZeroException))]
        public void DivideByZero()
        {
            Matchers.Same(calculator.Div(4, 0), 0);
        }
    }
}
