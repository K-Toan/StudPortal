using StudPortal.Server.Models;

namespace StudPortal.Server.Services.AccountService
{
    public interface IAccountService
    {
        public int GetAccountNumber();
        Task<IEnumerable<Account>> GetAllAccounts();
        Account GetAccountById(int id);
        Account GetAccountByUsername(string username);
        bool Add(Account account);
        bool Update(Account account);
        bool Delete(Account account);
        bool Clean();
        bool Save();
    }
}
