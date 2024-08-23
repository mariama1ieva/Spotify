using Domain.Entities;
using Repository.Repositories.Interfaces;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<List<AppUser>> GetAll()
        {
            return await _accountRepository.GetAll();
        }

        public async Task<IList<string>> GetRoles(AppUser user)
        {
            return await _accountRepository.GetRoles(user);
        }

    }
}
