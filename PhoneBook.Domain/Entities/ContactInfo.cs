namespace PhoneBook.Domain.Entities
{
    public class ContactInfo
    {
        public List<PhoneInfo>? PhoneNumber { get; set; }
        public List<EmailInfo>? Email { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }

    }
}
