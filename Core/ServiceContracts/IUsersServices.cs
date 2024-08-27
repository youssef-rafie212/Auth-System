using Core.Domain.Entities;
using Core.DTO.AccountsDtos;

namespace Core.ServiceContracts
{
    public interface IUsersServices
    {
        public Task DeleteUserAccount(DeleteUserAccountDto deleteUserAccountDto);

        public Task<ApplicationUser> GetUserByToken(GetUserByTokenDto getUserByTokenDto);

        public Task<ApplicationUser> GetUserByUsername(GetUserByUsernameDto getUserByUsernameDto);

        public Task UpdateEmail(UpdateEmailDto updateEmailDto);

        public Task UpdatePassword(UpdatePasswordDto updatePasswordDto);

        public Task UpdateUsername(UpdateUsernameDto updateUsernameDto);
    }
}
