using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUnit.Errors
{

    [Serializable]
    public class MatchedFailureException : Exception
    {
        public MatchedFailureException() { }
        public MatchedFailureException(string message) : base(message) { }
        public MatchedFailureException(string message, Exception inner) : base(message, inner) { }
        protected MatchedFailureException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
