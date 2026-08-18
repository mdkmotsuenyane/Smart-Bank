using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Text.RegularExpressions;

namespace SmartBank
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Simple in-memory attempt/lockout tracking for demo purposes
        private static Dictionary<string, int> failedAttempts = new Dictionary<string, int>();
        private static Dictionary<string, DateTime> lockouts = new Dictionary<string, DateTime>();
        private int loginAttempts = 0;
        private DateTime lastAttemptTime = DateTime.Now;

        public MainWindow()
        {
            InitializeComponent();
        }

        private bool IsRateLimited()
        {
            if (DateTime.Now - lastAttemptTime < TimeSpan.FromSeconds(10))
            {
                loginAttempts++;
                if (loginAttempts >= 5)
                {
                    MessageBox.Show("Too many login attempts. Please wait a few seconds.",
                                  "Rate Limited", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return true;
                }
            }
            else
            {
                loginAttempts = 1;
            }
            lastAttemptTime = DateTime.Now;
            return false;
        }

        private bool IsAccountLocked(string accountNumber)
        {
            if (lockouts.ContainsKey(accountNumber) && DateTime.Now < lockouts[accountNumber])
            {
                TimeSpan remaining = lockouts[accountNumber] - DateTime.Now;
                MessageBox.Show($"Account is temporarily locked. Please try again in {remaining.Minutes + 1} minute(s).",
                              "Account Locked", MessageBoxButton.OK, MessageBoxImage.Warning);
                return true;
            }

            if (lockouts.ContainsKey(accountNumber))
            {
                // Lockout period has expired
                lockouts.Remove(accountNumber);
                failedAttempts[accountNumber] = 0;
            }

            return false;
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string accountNumber = AccountNumberInput.Text.Trim();
                string pin = PinInput.Password;

                // Validate Account Number
                if (string.IsNullOrWhiteSpace(accountNumber))
                {
                    MessageBox.Show("Please enter your account number.", "Validation Error",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    AccountNumberInput.Focus();
                    return;
                }

                // Validate PIN
                if (string.IsNullOrWhiteSpace(pin))
                {
                    MessageBox.Show("Please enter your PIN.", "Validation Error",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    PinInput.Focus();
                    return;
                }

                int pinValue;
                if (!int.TryParse(pin, out pinValue))
                {
                    MessageBox.Show("PIN must contain numbers only.", "Validation Error",
                                  MessageBoxButton.OK, MessageBoxImage.Warning);
                    PinInput.Clear();
                    PinInput.Focus();
                    return;
                }

                // Check rate limiting
                if (IsRateLimited())
                {
                    return;
                }

                // Check if account is locked from too many failed PIN attempts
                if (IsAccountLocked(accountNumber))
                {
                    return;
                }

                // Search for the account from the collection of accounts
                BankAccount account = AccountRepository.FindAccount(accountNumber);

                if (account == null)
                {
                    MessageBox.Show("Account not found. Please check your account number.",
                                  "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    AccountNumberInput.Focus();
                    AccountNumberInput.SelectAll();
                    return;
                }

                // Check the PIN
                if (account.Pin == pin)
                {
                    failedAttempts[accountNumber] = 0;
                    loginAttempts = 0;

                    MessageBox.Show($"Welcome back, {account.AccountName}! Login successful.",
                                  "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Open the Banking Dashboard for the authenticated account
                    BankingDashboard dashboard = new BankingDashboard(account);
                    dashboard.Show();
                    this.Close();
                }
                else
                {
                    if (!failedAttempts.ContainsKey(accountNumber))
                    {
                        failedAttempts[accountNumber] = 0;
                    }
                    failedAttempts[accountNumber]++;

                    // Lock account after 5 failed attempts
                    if (failedAttempts[accountNumber] >= 5)
                    {
                        lockouts[accountNumber] = DateTime.Now.AddMinutes(5);
                        MessageBox.Show("Account has been locked for 5 minutes due to multiple failed PIN attempts.",
                                      "Account Locked", MessageBoxButton.OK, MessageBoxImage.Warning);
                        PinInput.Clear();
                        return;
                    }

                    int remainingAttempts = 5 - failedAttempts[accountNumber];
                    MessageBox.Show($"Incorrect PIN. {remainingAttempts} attempt(s) remaining before account lock.",
                                  "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    PinInput.Clear();
                    PinInput.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}",
                              "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Handle Enter key for account number field
        private void AccountNumberInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PinInput.Focus();
            }
        }

        // Handle Enter key for PIN field
        private void PinInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginBtn_Click(sender, e);
            }
        }

        // Shows the demo account numbers/PINs so a marker can log in and test the app
        private void DemoAccounts_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var message = new System.Text.StringBuilder();
            message.AppendLine("Demo accounts you can log in with:");
            message.AppendLine();

            foreach (var account in AccountRepository.Accounts.Values)
            {
                message.AppendLine($"{account.AccountNumber}  (PIN: {account.Pin})  -  {account.AccountName}");
            }

            MessageBox.Show(message.ToString(), "Demo Accounts", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
