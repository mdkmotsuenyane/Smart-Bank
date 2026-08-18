# 🏦 SmartBank

A secure desktop banking application built with **WPF (Windows Presentation Foundation)** and **.NET Framework 4.7.2**. SmartBank simulates core banking operations — login, deposits, withdrawals, transfers, balance checks, and transaction history — for a small set of predefined customer accounts.


## ✨ Features

- 🔐 **Login authentication** using Account Number + PIN, validated against a stored collection of accounts
- ⏱️ **Account lockout & rate limiting** after repeated failed PIN attempts
- 📋 **Banking Dashboard** displaying the logged-in customer's name, account number, and current balance
- 💰 **Deposit** money into the account
- 💳 **Withdraw** money, with insufficient-funds protection
- 🔄 **Transfer** funds to another valid account, updating both balances
- 📊 **Transaction history** for every deposit, withdrawal, and transfer (sent/received), with dates
- 🚪 **Logout**, returning to the login screen so another customer can sign in
- 🛡️ Structured exception handling throughout, with user-friendly `MessageBox` error messages

## 📁 Project Structure

```
SmartBank/
├── App.xaml / App.xaml.cs           # Application entry point
├── MainWindow.xaml / .xaml.cs       # Login window
├── BankingDashboard.xaml / .xaml.cs # Banking dashboard window
├── IBankOperations.cs               # Interface defining banking operations
├── BankAccount.cs                   # BankAccount class (implements IBankOperations)
├── AccountRepository.cs             # In-memory collection of predefined accounts
├── Properties/                      # Assembly info, resources, settings
└── SmartBank.csproj                 # Project file
```

## 🏗️ Architecture

### `IBankOperations` interface

Defines the contract every account must implement:

```csharp
void Deposit(double amount);
void Withdraw(double amount);
void Transfer(BankAccount receiver, double amount);
double CheckBalance();
void ViewTransactionHistory();
```

### `BankAccount` class

Implements `IBankOperations`. Stores `AccountNumber`, `AccountName`, `Pin`, and `Balance` as properties, and keeps a private transaction log. Deposit, withdraw, and transfer all validate their inputs and throw exceptions (e.g. negative amounts, insufficient funds, invalid recipient) rather than failing silently — the UI layer catches these and shows a `MessageBox`.

### `AccountRepository`

A static `Dictionary<string, BankAccount>` holding the demo accounts, searched during login and during transfers (to validate the recipient account number).

## 👥 Demo Accounts

| Account Number | PIN  | Account Holder            |
|----------------|------|----------------------------|
| ACC1001        | 1111 | Khololuhle Motsuenyane     |
| ACC1002        | 2222 | Bam Adebayo                |
| ACC1003        | 3333 | Demo User                  |
| ACC1004        | 4444 | Naledi Khumalo              |
| ACC1005        | 5555 | Sipho Dlamini               |

💡 You can also view this list from inside the app by clicking **"View Demo Accounts"** on the login screen.

## 🚀 Getting Started

### Prerequisites

- 🖥️ Windows with **.NET Framework 4.7.2** (or later) installed
- 🛠️ **Visual Studio 2019/2022** (Community edition or higher) with the *.NET desktop development* workload

### Running the app

1. Clone or download this repository.
2. Open `SmartBank.csproj` (or the solution containing it) in Visual Studio.
3. Press **F5** or click **Start** to build and run.
4. Log in with any of the demo account numbers and PINs above.

## 📖 Usage

1. 🔑 **Login** — enter an Account Number and PIN, then click **Sign In**.
2. 📋 **Dashboard** — view your balance, account number, and name.
3. 💰 **Deposit / Withdraw** — enter an amount (or use a quick-amount button) and click the relevant action button.
4. 🔄 **Transfer** — enter a recipient account number and an amount, then click **Transfer**.
5. 📊 **Transaction History** — click to refresh and view all deposits, withdrawals, and transfers on the account.
6. 🚪 **Logout** — returns to the login screen so a different customer can sign in.

## 🛡️ Validation & Exception Handling

The app handles the following situations gracefully, without crashing:

- ⚠️ Empty input fields
- ⚠️ Non-numeric input (letters instead of numbers)
- ⚠️ Invalid account number
- ⚠️ Incorrect PIN (with a lockout after repeated failures)
- ⚠️ Negative or zero deposit/withdrawal/transfer amounts
- ⚠️ Withdrawals or transfers exceeding the available balance
- ⚠️ Transfers to a non-existent account
- ⚠️ Unexpected runtime errors

## ✍🏾 Author

**Mokadi Motsuenyane**



## 📄 License

This project was created for educational purposes.
