using System;
using System.Collections;

public enum AccountType { Current, Savings, Deposit }

// Новый класс для хранения информации о банковской операции
public class BankTransaction
{
    private readonly DateTime transactionDate;
    private readonly double amount;

    public BankTransaction(double amount)
    {
        this.transactionDate = DateTime.Now;
        this.amount = amount;
    }

    public DateTime GetTransactionDate()
    {
        return transactionDate;
    }

    public double GetAmount()
    {
        return amount;
    }
}

public class BankAccount
{
    private static int idCounter = 1000;
    private int accountNumber;
    private double balance;
    private AccountType type;

    // Закрытое поле типа System.Collections.Queue для хранения операций
    private Queue transactions = new Queue();

    // Метод, генерирующий уникальный номер счета
    private static int GenerateAccountNumber()
    {
        idCounter = idCounter + 1;
        return idCounter;
    }

    // 1 Конструктор по умолчанию
    public BankAccount()
    {
        accountNumber = GenerateAccountNumber();
        balance = 0.0;
        type = AccountType.Current;
    }

    // 2 Конструктор для заполнения поля баланс
    public BankAccount(double initialBalance)
    {
        accountNumber = GenerateAccountNumber();
        balance = initialBalance;
        type = AccountType.Current;
    }

    // 3 Конструктор для заполнения поля тип банковского счета
    public BankAccount(AccountType accountType)
    {
        accountNumber = GenerateAccountNumber();
        balance = 0.0;
        type = accountType;
    }

    // 4 Конструктор для заполнения баланса и типа банковского счета
    public BankAccount(double initialBalance, AccountType accountType)
    {
        accountNumber = GenerateAccountNumber();
        balance = initialBalance;
        type = accountType;
    }

    public void Deposit(double amount)
    {
        balance = balance + amount;

        // Создаем транзакцию и добавляем в очередь
        BankTransaction transaction = new BankTransaction(amount);
        transactions.Enqueue(transaction);

        Console.WriteLine("Счет " + accountNumber + " пополнен на " + amount + " руб.");
    }

    public bool Withdraw(double amount)
    {
        if (amount <= balance)
        {
            balance = balance - amount;

            // Создаем транзакцию (со знаком минус для снятия) и добавляем в очередь
            BankTransaction transaction = new BankTransaction(-amount);
            transactions.Enqueue(transaction);

            Console.WriteLine("Со счета " + accountNumber + " успешно снято " + amount + " руб.");
            return true;
        }
        else
        {
            Console.WriteLine("Ошибка на счете " + accountNumber + ": Недостаточно средств для снятия " + amount + " руб.!");
            return false;
        }
    }

    // Перевод денег с другого счета (fromAccount) на текущий (this)
    public void Transfer(BankAccount fromAccount, double amount)
    {
        Console.WriteLine("\n--- Попытка перевода " + amount + " руб. со счета " + fromAccount.GetAccountNumber() + " на счет " + this.accountNumber + " ---");
        if (fromAccount.Withdraw(amount))
        {
            this.Deposit(amount);
            Console.WriteLine("Перевод успешно выполнен!");
        }
        else
        {
            Console.WriteLine("Перевод отменен из-за нехватки средств.");
        }
    }

    public int GetAccountNumber() { return accountNumber; }
    public double GetBalance() { return balance; }
    public AccountType GetAccountType() { return type; }

    public void PrintInfo()
    {
        Console.WriteLine("Номер счета: " + accountNumber);
        Console.WriteLine("Тип счета: " + type);
        Console.WriteLine("Текущий баланс: " + balance + " руб.");
        Console.WriteLine("-----------------------------------");
    }

    // метод для вывода истории транзакций
    public void PrintTransactions()
    {
        Console.WriteLine("История операций по счету " + accountNumber + ":");
        if (transactions.Count == 0)
        {
            Console.WriteLine("  Операций не проводилось.");
        }
        else
        {
            foreach (BankTransaction tr in transactions)
            {
                string operationType = tr.GetAmount() >= 0 ? "Пополнение" : "Снятие";
                Console.WriteLine("  [" + tr.GetTransactionDate() + "] " + operationType + ": " + tr.GetAmount() + " руб.");
            }
        }
        Console.WriteLine("-----------------------------------");
    }
}

class Program
{
    static void Main(string[] args)
    {
        BankAccount acc1 = new BankAccount(5000.00, AccountType.Current);
        BankAccount acc2 = new BankAccount(1000.00, AccountType.Savings);

        // Проводим несколько операций
        acc1.Deposit(1500.00);
        acc1.Withdraw(800.00);
        acc2.Transfer(acc1, 2000.00);

        Console.WriteLine();
        acc1.PrintInfo();
        acc1.PrintTransactions();

        acc2.PrintInfo();
        acc2.PrintTransactions();

        Console.ReadLine();
    }
}
