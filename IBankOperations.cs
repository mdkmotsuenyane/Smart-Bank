using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBank
{
    public interface IBankOperations
    {
        void Deposit(double amount);
        void Withdraw(double amount);
        void Transfer(BankAccount receiver, double amount);
        double CheckBalance();
        void ViewTransactionHistory();
    }
}
