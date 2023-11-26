using Microsoft.EntityFrameworkCore;
using StudPortal.Server.Data;
using StudPortal.Server.Models;

namespace StudPortal.Server.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _context;
        public AccountService(AppDbContext context)
        {
            _context = context;
        }

        public bool Add(Account account)
        {
            _context.Accounts.Add(account);
            return Save();
        }

        public bool Update(Account account)
        {
            _context.Accounts.Update(account);
            return Save();
        }

        public bool Delete(Account account)
        {
            _context.Accounts.Remove(account);
            return Save();
        }

        public bool Clean()
        {
            // Get all accounts
            var allAccounts = _context.Accounts.ToList();

            // Remove all accounts
            _context.Accounts.RemoveRange(allAccounts);
            return Save();
        }

        public int GetAccountNumber()
        {
            return _context.Accounts.ToList().Count;
        }
        public async Task<IEnumerable<Account>> GetAllAccounts()
        {
            return await _context.Accounts.ToListAsync();
        }

        public Account GetAccountById(int id)
        {
            return _context.Accounts.FirstOrDefault(a => a.Id == id);
        }

        public Account GetAccountByUsername(string username)
        {
            return _context.Accounts.FirstOrDefault(a => a.Username == username);
        }

        public bool Save()
        {
            _context.SaveChanges();
            return true;
        }
    }
}
