using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace DataBaseMigrator.Infrastructure
{
    public class NinjectDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernel)
        {
            this.kernel = kernel;
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
        private void AddBindings()
        {
          //  kernel.Bind<IRepository>().To<BookRepository>();
        }
    }
}