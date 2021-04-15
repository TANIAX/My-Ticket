using System;

namespace CleanArchitecture.Application.Common.Exceptions
{
    public class UnsupportedException : Exception
    {
        public UnsupportedException(string name, object key)
            : base($"Entity \"{name}\" ({key}) is not supported yet.")
        {
        }
    }
}
