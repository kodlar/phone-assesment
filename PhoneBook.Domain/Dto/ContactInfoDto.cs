namespace PhoneBook.Domain.Dto
{
    public class ContactInfoDto
    {
        public List<PhoneInfoDto>? PhoneNumber { get; set; }
        public List<EmailInfoDto>? Email { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
    }
}
