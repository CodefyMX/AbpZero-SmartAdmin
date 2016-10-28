using Abp.Localization;
using Cinotam.AbpModuleZero.EntityFramework;
using Cinotam.AbpModuleZero.Tests.FakeRequests;
using Cinotam.ModuleZero.AppModule.Languages;
using Cinotam.ModuleZero.AppModule.Languages.Dto;
using Shouldly;
using System.Data.Entity;
using System.Threading.Tasks;
using Xunit;

namespace Cinotam.AbpModuleZero.Tests.Languages
{
    public class LanguageAppService_Test : AbpModuleZeroTestBase
    {

        private readonly ILanguageAppService _languageAppService;

        public LanguageAppService_Test()
        {
            _languageAppService = Resolve<ILanguageAppService>();
        }

        [Fact]
        public async Task UpdateLanguageFromXml_Test()
        {
            LoginAsHostAdmin();
            await CreateFakeLang();
            await UsingDbContextAsync(async dbContext =>
            {
                var lang = await GetFakeLang(dbContext);

                lang.ShouldNotBeNull();

                lang.Name.ShouldBe("es");

                await _languageAppService.UpdateLanguageFromXml(lang.Name, "AbpModuleZero");

                var fakeRequest = FakeRequestHelper<LanguageTextsForEditRequest>.CreateDataTablesFakeRequestModel("");

                var languageTexts = _languageAppService.GetLocalizationTexts(fakeRequest);

                languageTexts.ShouldNotBeNull();
                languageTexts.data.ShouldNotBeNull();
                languageTexts.data.Length.ShouldBeGreaterThan(0);

            });
        }

        [Fact]
        public async Task AddLanguage_Test()
        {
            LoginAsHostAdmin();
            await CreateFakeLang();
            await UsingDbContextAsync(async dbContext =>
            {
                var lang = await GetFakeLang(dbContext);
                lang.ShouldNotBeNull();
                lang.Name.ShouldBe("es");
            });
        }
        public async Task CreateFakeLang()
        {
            await _languageAppService.AddLanguage(new LanguageInput()
            {
                DisplayName = "es",
                Icon = "",
                LangCode = "es"
            });
        }

        public async Task<ApplicationLanguage> GetFakeLang(AbpModuleZeroDbContext dbContext, string langCode = "es")
        {

            var lang = await dbContext.Languages.FirstOrDefaultAsync(a => a.Name == langCode);
            return lang;
        }

    }
}
