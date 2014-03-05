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
            Kernel = new StandardKernel();
            Kernel.Bind(x => x
                .FromAssembliesInPath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                .SelectAllClasses()
                .BindDefaultInterfaces());
        }
    }
}