namespace PhoneBook.Domain.Dto.QueryDto
{
    public class GetAllPhoneBookResponseDto
    {
        public string? Id { get; set; }        
        public string FirstName { get; set; }        
        public string LastName { get; set; }
        public string Company { get; set; }
        public ContactInfoDto? Contact { get; set; }
    }
}
