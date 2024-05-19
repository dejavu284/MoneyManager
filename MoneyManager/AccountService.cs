using MoneyManager.Data.Repositories;
using MoneyManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager
{
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
