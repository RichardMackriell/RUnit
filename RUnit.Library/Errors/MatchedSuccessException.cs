using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUnit.Errors
{

    [Serializable]
    public class MatchedSuccessException : Exception
    {
        public MatchedSuccessException() { }
        public MatchedSuccessException(string message) : base(message) { }
        public MatchedSuccessException(string message, Exception inner) : base(message, inner) { }
        protected MatchedSuccessException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
