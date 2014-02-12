using Castle.Windsor;
using LightPlayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightPlayerTests
{
    [TestClass]
    public class TestContext
    {
        protected IWindsorContainer WindsorContainer;

        [TestInitialize]
        public void SetupDependencies()
        {
            WindsorContainer = ViewModelLocator.SetupDependencyContainer();
            InitializeParticularDependencies();
        }

        public virtual void InitializeParticularDependencies()
        {
        }
    }
}
