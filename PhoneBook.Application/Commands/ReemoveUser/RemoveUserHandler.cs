using MediatR;
using PhoneBook.Infrastructure.Data.Interfaces;

namespace PhoneBook.Application.Commands.ReemoveUser
{
    public class RemoveUserHandler : IRequestHandler<RemoveUserRequest, RemoveUserResponse>
    {
        private readonly IPhoneBookRepository phoneBookRepository;

        public RemoveUserHandler(IPhoneBookRepository phoneBookRepository)
        {
            this.phoneBookRepository = phoneBookRepository;
        }

        public async Task<RemoveUserResponse> Handle(RemoveUserRequest request, CancellationToken cancellationToken)
        {
            var response = new RemoveUserResponse();
           

            var phoneBookItem = await this.phoneBookRepository.GetPhoneBookItemByIdAsync(request.UserId);

            if (phoneBookItem != null)
            {
                //kullanıcıyı sil
                phoneBookItem.IsDeleted = true;
                ////telefonları sil
                //foreach (var item in phoneBookItem.Contact?.PhoneNumber)
                //{
                //    item.IsDeleted = true;
                //}
                ////emailleri sil
                //foreach (var item in phoneBookItem.Contact?.Email)
                //{
                //    item.IsDeleted = true;
                //}

                response.Result = await this.phoneBookRepository.UpdateAsync(phoneBookItem);
             
            }

            return response;
        }
    }
}
