using System;
using System.Threading;
using Ninject;

namespace BasicAuthForMVC4.Common
{
    public static class ContainerManager
    {
        private static IKernel _container;
        private static readonly ReaderWriterLockSlim ContainerLock = new ReaderWriterLockSlim();

        public static IKernel Container
        {
            get
            {
                ContainerLock.EnterReadLock();
                try
                {
                    return _container;
                }
                finally
                {
                    ContainerLock.ExitReadLock();
                }
            }
            set
            {
                ContainerLock.EnterWriteLock();
                try
                {
                    if (_container != null)
                    {
                        throw new InvalidOperationException("Container already set");
                    }

                    _container = value;
                }
                finally
                {
                    ContainerLock.ExitWriteLock();
                }
            }
        }

        public static T Resolve<T>()
        {
            return Container.Get<T>();
        }
    }
}
