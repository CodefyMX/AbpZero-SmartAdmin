using Cinotam.Cms.FileSystemTemplateProvider.Provider;
using Shouldly;
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
        public void TestFileSystem()
        {
            var result = _fileSystemTemplateProvider.GetTemplateContent("");
            result.ShouldNotBeNull();
        }
    }
}
