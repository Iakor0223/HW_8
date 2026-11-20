using System;
using System.Collections;
using System.IO;

public enum AccountType { Current, Savings, Deposit }

public class BankTransaction
{
    private readonly DateTime transactionDate;
    private readonly double amount;

    public BankTransaction(double amount)
    {
        this.transactionDate = DateTime.Now;
        this.amount = amount;
    }

    public DateTime GetTransactionDate() { return transactionDate; }
    public double GetAmount() { return amount; }
}

public class BankAccount : IDisposable
{
    private static int idCounter = 1000;
    private int accountNumber;
    private double balance;
    private AccountType type;
    private Queue transactions = new Queue();

    private static int GenerateAccountNumber()
    {
        return ++idCounter;
    }

    public BankAccount()
    {
        accountNumber = GenerateAccountNumber();
    }

    public BankAccount(double initialBalance)
    {
        accountNumber = GenerateAccountNumber();
        balance = initialBalance;
    }

    public BankAccount(AccountType accountType)
    {
        accountNumber = GenerateAccountNumber();
        type = accountType;
    }

    public BankAccount(double initialBalance, AccountType accountType)
    {
        accountNumber = GenerateAccountNumber();
        balance = initialBalance;
        type = accountType;
    }

    public void Deposit(double amount)
    {
        balance += amount;
        transactions.Enqueue(new BankTransaction(amount));
    }

    public bool Withdraw(double amount)
    {
        if (amount <= balance)
        {
            balance -= amount;
            transactions.Enqueue(new BankTransaction(-amount));
            return true;
        }
        return false;
    }

    public void Transfer(BankAccount fromAccount, double amount)
    {
        if (fromAccount.Withdraw(amount))
        {
            this.Deposit(amount);
        }
    }

    // Метод Dispose: записывает очередь проводок в файл
    public void Dispose()
    {
        StreamWriter writer = new StreamWriter("transactions_" + accountNumber + ".txt");

        while (transactions.Count > 0)
        {
            BankTransaction tr = (BankTransaction)transactions.Dequeue();
            writer.WriteLine(tr.GetTransactionDate() + " | Сумма: " + tr.GetAmount());
        }

        writer.Close();
        GC.SuppressFinalize(this);
    }

    public void PrintInfo()
    {
        Console.WriteLine("Счет: " + accountNumber + ", Тип: " + type + ", Баланс: " + balance);
    }
}

class Program
{
    static void Main()
    {
        BankAccount acc1 = new BankAccount(5000, AccountType.Current);
        BankAccount acc2 = new BankAccount(1000, AccountType.Savings);

        acc1.Deposit(1500);
        acc1.Withdraw(800);
        acc2.Transfer(acc1, 2000);

        acc1.PrintInfo();
        acc2.PrintInfo();

        // Сохраняем транзакции в файлы
        acc1.Dispose();
        acc2.Dispose();

        Console.WriteLine("Транзакции записаны в файлы.");
        Console.ReadLine();
    }
}
