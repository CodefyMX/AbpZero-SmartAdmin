namespace Cinotam.ModuleZero.AppModule.Users.Dto
{
    public class UpdateProfilePictureInput
    {
        public long UserId { get; set; }
        public string ImageUrl { get; set; }
        public bool StoredInCdn { get; set; }
    }
}
