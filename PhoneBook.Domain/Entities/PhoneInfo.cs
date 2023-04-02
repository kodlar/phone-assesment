using PhoneBook.Domain.Enums;

namespace PhoneBook.Domain.Entities
{
    public class PhoneInfo
    {
        public string Id { get; set; }
        public bool IsSelected { get; set; }
        public string PhoneNumber { get; set; }
        public int CountryCode { get; set; }
        public bool IsDeleted { get; set; }
        public PhoneEnum Type  { get; set; }
    }
}
