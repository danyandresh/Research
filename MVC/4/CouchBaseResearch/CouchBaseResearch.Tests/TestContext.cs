using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Ninject.Extensions.Conventions;

namespace CouchBaseResearch.Tests
{
    public abstract class TestContext
    {
        protected StandardKernel Kernel { get; private set; }

        [TestInitialize]
        public void Initialize()
        {
            Kernel = SetupKernel();
        }

        protected StandardKernel SetupKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind(x => x
                .FromAssembliesInPath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                .SelectAllClasses()
                .BindDefaultInterfaces());

            return kernel;
        }
    }
}