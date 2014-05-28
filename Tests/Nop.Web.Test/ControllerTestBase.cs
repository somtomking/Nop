using Nop.Tests;
using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Nop.Web.Test
{
    [TestFixture]
    public class ControllerTestBase : TestsBase
    {
        protected HttpContextBase _mockedhttpContext;
        protected HttpContextBase _mockedHttpRequest;

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
            base.SetUp();

            _mockedhttpContext = mocks.DynamicMock<HttpContextBase>();
            _mockedHttpRequest = mocks.DynamicMock<HttpRequestBase>();

            var result = SetupResult.For(mockedhttpContext.Request).Return(_mockedHttpRequest);
            
        }
        /// <summary>
        /// 方法执行之后
        /// </summary>
        public override void TearDown()
        {
            base.TearDown();
        }
    }
}
