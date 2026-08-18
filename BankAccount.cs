using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBank
{
    public class BankAccount : IBankOperations
    {
        //properties 

        // Add transaction history list
        private List<string> transactionHistory = new List<string>();

        public BankAccount(string accountNumber, string accountName, string pin, double balance)
        {
            AccountNumber = accountNumber;
            AccountName = accountName;
            Pin = pin;
            Balance = balance;
        }

        public BankAccount()
        {
            Balance = 0;
        }

        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string Pin { get; set; }
        public double Balance { get; set; }

        // Read-only access to the transaction log so the UI can bind to it
        public List<string> TransactionHistory
        {
            get { return transactionHistory; }
        }

        public void Deposit(double amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Deposit amount must be greater than zero.");
            }

            Balance += amount;
            transactionHistory.Add($"{DateTime.Now:dd/MM/yyyy} Deposit: R{amount}");
        }

        public void Withdraw(double amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Withdrawal amount must be greater than zero.");
            }

            if (amount > Balance)
            {
                throw new InvalidOperationException("Insufficient funds for this withdrawal.");
            }

            Balance -= amount;
            transactionHistory.Add($"{DateTime.Now:dd/MM/yyyy} Withdraw: R{amount}");
        }

        public void Transfer(BankAccount receiver, double amount)
        {
            if (receiver == null)
            {
                throw new ArgumentException("Recipient account does not exist.");
            }

            if (amount <= 0)
            {
                throw new ArgumentException("Transfer amount must be greater than zero.");
            }

            if (amount > Balance)
            {
                throw new InvalidOperationException("Insufficient funds for this transfer.");
            }

            Balance -= amount;
            receiver.Balance += amount;

            transactionHistory.Add($"{DateTime.Now:dd/MM/yyyy} Transfer to {receiver.AccountNumber}: R{amount}");
            receiver.transactionHistory.Add($"{DateTime.Now:dd/MM/yyyy} Received from {AccountNumber}: R{amount}");
        }

        public double CheckBalance()
        {
            return Balance;
        }

        public void ViewTransactionHistory()
        {
            foreach (var transaction in transactionHistory)
            {
                Console.WriteLine(transaction);
            }
        }
    }
}
