using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SwapIt.Data.Helpers.Exceptions
{
    public class NoClaimException : Exception
    {
        public NoClaimException(string message) : base(message)
        {
        }

        protected NoClaimException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
