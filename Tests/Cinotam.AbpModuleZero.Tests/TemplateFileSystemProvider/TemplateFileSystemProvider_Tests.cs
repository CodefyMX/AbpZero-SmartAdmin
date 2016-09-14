using Cinotam.Cms.FileSystemTemplateProvider.Provider;
using FakeItEasy;
using Shouldly;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Web;
using Xunit;

namespace Cinotam.AbpModuleZero.Tests.TemplateFileSystemProvider
{
    public class TemplateFileSystemProvider_Tests : AbpModuleZeroTestBase
    {
        private readonly IFileSystemTemplateProvider _fileSystemTemplateProvider;
        public TemplateFileSystemProvider_Tests()
        {
            _fileSystemTemplateProvider = Resolve<IFileSystemTemplateProvider>();
        }

        [Fact]
        public async Task TestFileSystem()
        {
            FakeHttpContext();
            var result = await _fileSystemTemplateProvider.GetTemplateContent("Simple");
            result.ShouldNotBeNull();
        }
        public static HttpContextBase FakeHttpContext()
        {
            var context = A.Fake<HttpContextBase>();
            var request = A.Fake<HttpRequestBase>();
            var response = A.Fake<HttpResponseBase>();
            var session = A.Fake<HttpSessionStateBase>();
            var server = A.Fake<HttpServerUtilityBase>();

            A.CallTo(() => request.QueryString).Returns(new NameValueCollection());
            A.CallTo(() => request.Form).Returns(new NameValueCollection());
            A.CallTo(() => request.Headers).Returns(new NameValueCollection());

            A.CallTo(() => context.Request).Returns(request);
            A.CallTo(() => context.Response).Returns(response);
            A.CallTo(() => context.Session).Returns(session);
            A.CallTo(() => context.Server).Returns(server);

            return context;
        }
    }
}
