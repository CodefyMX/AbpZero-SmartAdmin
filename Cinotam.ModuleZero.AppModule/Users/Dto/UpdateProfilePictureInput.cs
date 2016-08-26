using System.Web;

namespace Cinotam.ModuleZero.AppModule.Users.Dto
{
    public class UpdateProfilePictureInput
    {
        public long UserId { get; set; }
        public HttpPostedFileBase Image { get; set; }
    }
}
