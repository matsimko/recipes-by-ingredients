using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RbiData.Services;
public class OwnershipException : Exception
{
    public OwnershipException()
    {
    }

    public OwnershipException(string? message) : base(message)
    {
    }

    public OwnershipException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected OwnershipException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
