using RUnit.Introspection;
using RUnit.Runner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitUnderTest;

namespace RUnit.Driver
{
    class Program
    {
        static void Main(string[] args)
        {
            TestRunner.Run(AppDomain.CurrentDomain.GetAssemblies());
            Console.Read();
        }
    }
}
