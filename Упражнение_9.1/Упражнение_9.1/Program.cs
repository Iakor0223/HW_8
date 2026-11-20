using System;

public enum AccountType { Current, Savings, Deposit }

public class BankAccount
{
    private static int idCounter = 1000;
    private int accountNumber;
    private double balance;
    private AccountType type;

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
        Console.WriteLine("Счет " + accountNumber + " пополнен на " + amount + " руб.");
    }

    public bool Withdraw(double amount)
    {
        if (amount <= balance)
        {
            balance = balance - amount;
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
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Проверка всех четырех конструкторов
        BankAccount accDefault = new BankAccount();
        BankAccount accBalanceOnly = new BankAccount(3500.00);
        BankAccount accTypeOnly = new BankAccount(AccountType.Deposit);
        BankAccount accFull = new BankAccount(5000.00, AccountType.Savings);

        Console.WriteLine("Созданные счета:");
        accDefault.PrintInfo();
        accBalanceOnly.PrintInfo();
        accTypeOnly.PrintInfo();
        accFull.PrintInfo();

        // Проверка перевода между счетами
        accDefault.Transfer(accFull, 2000.00);

        Console.WriteLine("\nСостояние счетов после перевода:");
        accDefault.PrintInfo();
        accFull.PrintInfo();

        Console.ReadLine();
    }
}

