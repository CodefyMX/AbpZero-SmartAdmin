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

            var tenancyNameWithNoCrOne = _multiTenancyHelper.SetCurrentTenancy("https://www.cinotam.geronimo.host.wow.com");
            tenancyNameWithNoCrOne.ShouldBe("cinotam");

            var tenancyNameWithNoCrTwo = _multiTenancyHelper.SetCurrentTenancy("https://www.cinotam.com");
            tenancyNameWithNoCrTwo.ShouldBe("cinotam");
            var tenancyNameWithNoCrThree = _multiTenancyHelper.SetCurrentTenancy("https://www.cinotam.com.mx");
            tenancyNameWithNoCrThree.ShouldBe("cinotam");


            var testLocal = _multiTenancyHelper.SetCurrentTenancy("cinotam.localhost");
            testLocal.ShouldBe("cinotam");
            var testLocalAlt = _multiTenancyHelper.SetCurrentTenancy("cinotam.localhost:61815/Page/main-frame-cpu-dual-core");
            testLocalAlt.ShouldBe("cinotam");

        }
    }
}
