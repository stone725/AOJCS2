using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AojCs
{
  class AojSubmitException : Exception
  {
    public AojSubmitException()
    {
    }

    public AojSubmitException(string message) : base(message)
    {
    }

    public AojSubmitException(string message, Exception innerException) : base(message, innerException)
    {
    }
  }
}
