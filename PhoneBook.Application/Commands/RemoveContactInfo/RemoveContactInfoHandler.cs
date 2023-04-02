using MediatR;
using PhoneBook.Infrastructure.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Application.Commands.RemoveContactInfo
{
    public class RemoveContactInfoHandler : IRequestHandler<RemoveContactInfoRequest, RemoveContactInfoResponse>
    {
        private readonly IPhoneBookRepository phoneBookRepository;

        public RemoveContactInfoHandler(IPhoneBookRepository phoneBookRepository)
        {
            this.phoneBookRepository = phoneBookRepository;
        }

        public async Task<RemoveContactInfoResponse> Handle(RemoveContactInfoRequest request, CancellationToken cancellationToken)
        {
            var response = new RemoveContactInfoResponse();

            var phoneBookItem = await this.phoneBookRepository.GetPhoneBookItemByIdAsync(request.UserId);

            if (phoneBookItem != null)
            {
                if (request.PhoneIds != null && request.PhoneIds.Any())
                {
                    foreach (var item in request.PhoneIds)
                    {
                        var phoneBookContactItemPhoneNumber = phoneBookItem.Contact.PhoneNumber.FirstOrDefault(x => x.Id == item);
                        phoneBookContactItemPhoneNumber.IsDeleted = true;
                    }
                    
                }

                if(request.EmailIds != null && request.EmailIds.Any())
                {
                    foreach (var item in request.EmailIds)
                    {
                        var phoneBookContactItemEmailNumber = phoneBookItem.Contact.Email.FirstOrDefault(y => y.Id == item);
                        phoneBookContactItemEmailNumber.IsDeleted = true;
                    }
                }

                response.Result = await this.phoneBookRepository.UpdateAsync(phoneBookItem);
            }
                return response;
        }
    }
}
