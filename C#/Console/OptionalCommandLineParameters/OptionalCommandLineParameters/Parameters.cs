using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionalCommandLineParameters
{
    class Parameters
    {
        internal static Parameters Create(bool atomic = true)
        {
            throw new NotImplementedException();
        }

        internal void AddOptional(string parameters, string defaultValue)
        {
            throw new NotImplementedException();
        }

        internal void Add(Parameters b)
        {
            throw new NotImplementedException();
        }

        internal void Add(params string[] stringParameters)
        {
            throw new NotImplementedException();
        }

        internal void BindAction(Action<IDictionary<string, string>> dummyOperation)
        {
            throw new NotImplementedException();
        }

        internal static Parameters CreateOptional()
        {
            throw new NotImplementedException();
        }
    }
}
