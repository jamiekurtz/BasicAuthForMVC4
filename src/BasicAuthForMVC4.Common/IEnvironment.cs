namespace BasicAuthForMVC4.Common
{
    public interface IEnvironment
    {
        int ProcessorCount { get; }
        string MachineName { get; }
        string CurrentDirectory { get; }
    }
}