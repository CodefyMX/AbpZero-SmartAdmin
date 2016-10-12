using Cinotam.AbpModuleZero.Tools.DatatablesJsModels.GenericTypes;
using Moq;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace Cinotam.AbpModuleZero.Tests.FakeRequests
{
    public class FakeRequestHelper
    {
        public static RequestModel<object> CreateDataTablesFakeRequestModel()
        {
            return new RequestModel<object>()
            {
                draw = 1,
                length = 10,
                PropOrd = "Id",
                PropToSearch = "Id",
                PropToSort = "Id",
                PropSort = 1,
                SearchCol = "Id",
                search = new Dictionary<string, string>() { { "value", "" } }
            };
        }

        public static Mock<HttpPostedFileBase> FakeFile()
        {
            var postedFile = new Mock<HttpPostedFileBase>();

            using (var stream = new MemoryStream())
            using (var bmp = new Bitmap(1, 1))
            {
                var graphics = Graphics.FromImage(bmp);
                graphics.FillRectangle(Brushes.Black, 0, 0, 1, 1);
                bmp.Save(stream, ImageFormat.Jpeg);

                postedFile.Setup(pf => pf.InputStream).Returns(stream);

                // Assert something with postedFile here  
                return postedFile;
            }
        }
    }
}
