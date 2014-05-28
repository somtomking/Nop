using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Controllers;
using Nop.Tests;
using NUnit.Framework;
using Nop.Core.Infrastructure;
using Nop.Tests.Common;
namespace Nop.Web.Test
{
    [TestFixture]
    public class ShoppingCartControllerTests : ControllerTestBase
    {
        public override void TestFixtureSetUp()
        {
          EngineContext.Initialize(false);


        }
        public override void TestFixtureTearDown()
        {

        }
        public override void SetUp()
        {

        }
        public override void TearDown()
        {

        }
        [Test]
        public void Test()
        {
            var controller = EngineContext.Current.Resolve<ShoppingCartController>();
            
            
             
        }
    }
}
