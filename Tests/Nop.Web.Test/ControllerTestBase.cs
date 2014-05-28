using Nop.Tests;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Web.Test
{
     [TestFixture]
    public class ControllerTestBase : TestsBase
    {
          /// <summary>
        /// 测试类执行之前
        /// </summary>
        [TestFixtureSetUp]
        public virtual void TestFixtureSetUp()
        {

        }
        /// <summary>
        /// 测试类执行之后
        /// </summary>
        [TestFixtureTearDown]
        public virtual void TestFixtureTearDown()
        {

        }
         /// <summary>
         /// 方法执行之前
         /// </summary>
        public override void SetUp()
        {

        }
         /// <summary>
         /// 方法执行之后
         /// </summary>
        public override void TearDown()
        {

        }
    }
}
