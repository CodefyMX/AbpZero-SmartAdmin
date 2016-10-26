using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinotam.ModuleZero.AppModule.MultiTenancy.MultiTenancyHelper;
using Shouldly;
using Xunit;

namespace Cinotam.AbpModuleZero.Tests.MultiTenancy
{
    public class MultiTenancyHelper_Test : AbpModuleZeroTestBase
    {
        private readonly IMultiTenancyHelper _multiTenancyHelper;
        public MultiTenancyHelper_Test()
        {
            _multiTenancyHelper = Resolve<IMultiTenancyHelper>();
        }

        [Fact]
        public void GetTenancyNameByUrl_Test()
        {
            var tenancyName = _multiTenancyHelper.SetCurrentTenancy("http://www.cinotam.localhost.com");
            tenancyName.ShouldBe("cinotam");
            var tenancyNameWithNoTw = _multiTenancyHelper.SetCurrentTenancy("http://cinotam.localhost.com");
            tenancyNameWithNoTw.ShouldBe("cinotam");
            var tenancyNameWithHttps = _multiTenancyHelper.SetCurrentTenancy("https://cinotam.localhost.com");
            tenancyNameWithHttps.ShouldBe("cinotam");
            var tenancyNameWithNoPr = _multiTenancyHelper.SetCurrentTenancy("www.cinotam.localhost.com");
            tenancyNameWithNoPr.ShouldBe("cinotam");

        }
    }
}
