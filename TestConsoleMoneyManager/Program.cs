using System.Security.Principal;
using MoneyManager.Models;
using MoneyManager.Data.Repositories;
using MoneyManager.Data;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace TestConsoleMoneyManager
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql("Host=localhost;Database=MoneyManager;Username=postgres;Password=1111")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var accountRepository = new AccountRepository(context);
                var accountService = new AccountService(accountRepository);

                await accountService.Run();
            }
        }

    }
    public class AccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task Run()
        {
            // Создание нового аккаунта
            var newAccount = new Account { AccountBalance = 1000.00m, CurrencyId = 1 };
            await _accountRepository.AddAccountAsync(newAccount);

            // Чтение всех аккаунтов
            var accounts = await _accountRepository.GetAllAccountsAsync();
            Console.WriteLine("Accounts: " + accounts.Count());

            // Обновление аккаунта
            var accountToUpdate = accounts.First();
            accountToUpdate.AccountBalance += 500.00m;
            await _accountRepository.UpdateAccountAsync(accountToUpdate);

            // Удаление аккаунта
            var accountIdToDelete = accountToUpdate.AccountId;
            await _accountRepository.DeleteAccountAsync(accountIdToDelete);

            // Чтение всех аккаунтов после удаления
            accounts = await _accountRepository.GetAllAccountsAsync();
            Console.WriteLine("Accounts after deletion: " + accounts.Count());
        }
    }
}
