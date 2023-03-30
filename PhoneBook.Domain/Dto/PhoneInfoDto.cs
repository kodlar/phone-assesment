using PhoneBook.Domain.Enums;

namespace PhoneBook.Domain.Dto
{
    public class PhoneInfoDto
    {
        public bool IsSelected { get; set; }
        public string PhoneNumber { get; set; }
        public int CountryCode { get; set; }  
        /// <summary>
        /// 0-Home 1-Mobile 2-Work
        /// </summary>
        public PhoneEnum Type { get; set; }
    }
}
