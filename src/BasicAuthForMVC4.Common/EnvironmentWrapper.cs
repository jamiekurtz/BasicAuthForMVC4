using System;

namespace BasicAuthForMVC4.Common
{
    public class EnvironmentWrapper : IEnvironment
    {
        public int ProcessorCount
        {
            get { return Environment.ProcessorCount; }
        }

        public string MachineName
        {
            get { return Environment.MachineName; }
        }

        public string CurrentDirectory
        {
            get { return Environment.CurrentDirectory; }
        }
    }
}
