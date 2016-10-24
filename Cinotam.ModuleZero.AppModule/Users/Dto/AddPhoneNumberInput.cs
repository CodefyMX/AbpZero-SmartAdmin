using System.ComponentModel.DataAnnotations;

namespace Cinotam.ModuleZero.AppModule.Users.Dto
{
    public class AddPhoneNumberInput
    {
        public long UserId { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public string CountryPhoneCode { get; set; }
        public string CountryCode { get; set; }
    }
}
