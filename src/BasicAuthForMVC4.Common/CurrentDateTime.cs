using System;

namespace BasicAuthForMVC4.Common
{
    public class CurrentDateTime : ICurrentDateTime
    {
        public DateTime Now
        {
            get { return DateTime.Now; }
        }

        public DateTime Today
        {
            get { return DateTime.Today; }
        }
    }
}
