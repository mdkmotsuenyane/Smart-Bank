using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBank
{
    // Holds the collection of predefined customer accounts.
    // Both the Login Window and the Banking Dashboard (for transfers)
    // search this same collection.
    public static class AccountRepository
    {
        private static Dictionary<string, BankAccount> accounts = new Dictionary<string, BankAccount>();

        static AccountRepository()
        {
            accounts.Add("ACC1001", new BankAccount("ACC1001", "Khololuhle Motsuenyane", "1111", 15000.00));
            accounts.Add("ACC1002", new BankAccount("ACC1002", "Bam Adebayo", "2222", 25000.50));
            accounts.Add("ACC1003", new BankAccount("ACC1003", "Adriaan Koek", "3333", 5000.00));
            accounts.Add("ACC1004", new BankAccount("ACC1004", "Naledi Khumalo", "4444", 8000.00));
            accounts.Add("ACC1005", new BankAccount("ACC1005", "Sipho Dlamini", "5555", 12000.00));
        }

        public static Dictionary<string, BankAccount> Accounts
        {
            get { return accounts; }
        }

        // Search the collection for an account by account number
        public static BankAccount FindAccount(string accountNumber)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
            {
                return null;
            }

            if (accounts.ContainsKey(accountNumber))
            {
                return accounts[accountNumber];
            }

            return null;
        }
    }
}
