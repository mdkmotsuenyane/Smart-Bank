using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SmartBank
{
    /// <summary>
    /// Interaction logic for BankingDashboard.xaml
    /// </summary>
    public partial class BankingDashboard : Window
    {
        // The authenticated customer's account
        public BankAccount account;

        public BankingDashboard(BankAccount loggedInAccount)
        {
            InitializeComponent();
            account = loggedInAccount;
            RefreshDashboard();
        }

        // Updates the customer info cards and the transaction list on screen
        private void RefreshDashboard()
        {
            AccountNameDisplay.Text = account.AccountName;
            AccountNumberDisplay.Text = account.AccountNumber;
            BalanceDisplay.Text = $"R{account.Balance:N2}";

            TransactionList.ItemsSource = null;
            TransactionList.ItemsSource = account.TransactionHistory;
        }

        private void DepositButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(AmountInput.Text))
                {
                    throw new Exception("Please enter an amount to deposit.");
                }

                double amount;
                if (!double.TryParse(AmountInput.Text, out amount))
                {
                    throw new Exception("Please enter a valid numeric amount.");
                }

                account.Deposit(amount);
                RefreshDashboard();

                MessageBox.Show($"R{amount} deposited successfully. New balance: R{account.Balance:N2}",
                              "Deposit Successful", MessageBoxButton.OK, MessageBoxImage.Information);

                AmountInput.Text = "0.00";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Deposit Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void WithdrawButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(AmountInput.Text))
                {
                    throw new Exception("Please enter an amount to withdraw.");
                }

                double amount;
                if (!double.TryParse(AmountInput.Text, out amount))
                {
                    throw new Exception("Please enter a valid numeric amount.");
                }

                account.Withdraw(amount);
                RefreshDashboard();

                MessageBox.Show($"R{amount} withdrawn successfully. New balance: R{account.Balance:N2}",
                              "Withdrawal Successful", MessageBoxButton.OK, MessageBoxImage.Information);

                AmountInput.Text = "0.00";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Withdrawal Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TransferButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string recipientAccountNumber = RecipientAccountInput.Text.Trim();

                if (string.IsNullOrWhiteSpace(recipientAccountNumber))
                {
                    throw new Exception("Please enter a recipient account number.");
                }

                if (string.IsNullOrWhiteSpace(AmountInput.Text))
                {
                    throw new Exception("Please enter an amount to transfer.");
                }

                double amount;
                if (!double.TryParse(AmountInput.Text, out amount))
                {
                    throw new Exception("Please enter a valid numeric amount.");
                }

                if (recipientAccountNumber == account.AccountNumber)
                {
                    throw new Exception("You cannot transfer money to your own account.");
                }

                // Validate the recipient account
                BankAccount receiver = AccountRepository.FindAccount(recipientAccountNumber);
                if (receiver == null)
                {
                    throw new Exception("Recipient account not found. Please check the account number.");
                }

                account.Transfer(receiver, amount);
                RefreshDashboard();

                MessageBox.Show($"R{amount} transferred to {receiver.AccountName} ({receiver.AccountNumber}) successfully.",
                              "Transfer Successful", MessageBoxButton.OK, MessageBoxImage.Information);

                AmountInput.Text = "0.00";
                RecipientAccountInput.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Transfer Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TranstactionButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                account.ViewTransactionHistory();
                RefreshDashboard();

                if (account.TransactionHistory.Count == 0)
                {
                    MessageBox.Show("No transactions have been made on this account yet.",
                                  "Transaction History", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void QuickAmount_Click1(object sender, RoutedEventArgs e)
        {
            double amount;
            double.TryParse(AmountInput.Text, out amount);
            amount += 20.0;
            AmountInput.Text = amount.ToString();
        }

        private void QuickAmount_Click2(object sender, RoutedEventArgs e)
        {
            double amount;
            double.TryParse(AmountInput.Text, out amount);
            amount += 50.0;
            AmountInput.Text = amount.ToString();
        }

        private void QuickAmount_Click3(object sender, RoutedEventArgs e)
        {
            double amount;
            double.TryParse(AmountInput.Text, out amount);
            amount += 100.0;
            AmountInput.Text = amount.ToString();
        }

        private void QuickAmount_Click4(object sender, RoutedEventArgs e)
        {
            double amount;
            double.TryParse(AmountInput.Text, out amount);
            amount += 200.0;
            AmountInput.Text = amount.ToString();
        }

        //logged out
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
