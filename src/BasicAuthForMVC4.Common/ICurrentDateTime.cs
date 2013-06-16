using System;

namespace BasicAuthForMVC4.Common
{
    public interface ICurrentDateTime
    {
        DateTime Now { get; }
        DateTime Today { get; }
    }
}