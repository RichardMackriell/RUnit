using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitUnderTest
{
    public interface IExampleService
    {
        string GetMessage();
        Person GetPerson();
        int GetValue();
    }
}
