using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitUnderTest
{
    public class DependantType
    {
        private readonly IExampleService service;

        public DependantType(IExampleService service)
        {
            this.service = service;
        }
        public string GetMessage()
        {
            return service.GetMessage();
        }
        public Person GetPerson()
        {
            return service.GetPerson();
        }
        public int GetValue()
        {
            return service.GetValue();
        }
    }
}
