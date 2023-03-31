using MediatR;

namespace PhoneBook.Application.Queries.GetPhoneBookById
{
    public class GetPhoneBookByIdRequest: IRequest<GetPhoneBookByIdResponse>
    {
        public string Id { get; set; }

    }
}
