using System;
using System.Web.Mvc;
using Ninject;

namespace BasicAuthForMVC4.Web.Common
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel _container;

        public IKernel Container
        {
            get { return _container; }
        }

        public NinjectControllerFactory(IKernel container)
        {
            _container = container;
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            return
                controllerType == null
                    ? null
                    : (IController) _container.Get(controllerType);
        }
    }
}