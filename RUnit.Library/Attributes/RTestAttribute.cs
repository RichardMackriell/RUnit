using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUnit
{
    [System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class RTestAttribute : Attribute
    {
        private readonly Type expectedException;
        public bool ExpectsException { get { return expectedException != null; } }

        public Type ExceptionType { get { return expectedException; } }

        public RTestAttribute(Type ExpectedException = null)
        {           
            this.expectedException = ExpectedException;
        }
    }
}
