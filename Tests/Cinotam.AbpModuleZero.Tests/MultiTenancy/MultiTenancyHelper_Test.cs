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
            var tenancyName = _multiTenancyHelper.GetCurrentTenancyName("http://www.cinotam.localhost.com");
            tenancyName.ShouldBe("cinotam");
            var tenancyNameWithNoTw = _multiTenancyHelper.GetCurrentTenancyName("http://cinotam.localhost.com");
            tenancyNameWithNoTw.ShouldBe("cinotam");
            var tenancyNameWithHttps = _multiTenancyHelper.GetCurrentTenancyName("https://cinotam.localhost.com");
            tenancyNameWithHttps.ShouldBe("cinotam");
            var tenancyNameWithNoPr = _multiTenancyHelper.GetCurrentTenancyName("www.cinotam.localhost.com");
            tenancyNameWithNoPr.ShouldBe("cinotam");

            var tenancyNameWithNoCrOne = _multiTenancyHelper.GetCurrentTenancyName("https://www.cinotam.geronimo.host.wow.com");
            tenancyNameWithNoCrOne.ShouldBe("cinotam");

            var tenancyNameWithNoCrTwo = _multiTenancyHelper.GetCurrentTenancyName("https://www.cinotam.com");
            tenancyNameWithNoCrTwo.ShouldBe("cinotam");
            var tenancyNameWithNoCrThree = _multiTenancyHelper.GetCurrentTenancyName("https://www.cinotam.com.mx");
            tenancyNameWithNoCrThree.ShouldBe("cinotam");


            var testLocal = _multiTenancyHelper.GetCurrentTenancyName("cinotam.localhost");
            testLocal.ShouldBe("cinotam");
            var testLocalAlt = _multiTenancyHelper.GetCurrentTenancyName("cinotam.localhost:61815/Page/main-frame-cpu-dual-core");
            testLocalAlt.ShouldBe("cinotam");

        }
    }
}
