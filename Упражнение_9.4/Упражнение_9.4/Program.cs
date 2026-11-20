using System;
using System.Collections.Generic;

// Статусы проекта
enum ProjectStatus
{
    Project,    // Проект
    Execution,  // Исполнение
    Closed      // Закрыт
}

// Статусы задачи
enum TaskStatus
{
    Assigned,    // Назначена
    InProgress,  // В работе
    UnderReview, // На проверке
    Completed    // Выполнена
}

// Класс человека
class Person
{
    public string Name;

    public Person(string name)
    {
        Name = name;
    }
}

// Класс Отчёт
class Report
{
    public string Text;
    public DateTime ExecutionDate;
    public Person Executor;

    public Report(string text, Person executor)
    {
        Text = text;
        ExecutionDate = DateTime.Now;
        Executor = executor;
    }
}

// Класс Задача
class ProjectTask
{
    public string Description;
    public DateTime Deadline;
    public Person Initiator;
    public Person Executor;
    public TaskStatus Status;
    public Report TaskReport; // Один отчёт по одной задаче

    public ProjectTask(string description, DateTime deadline, Person initiator, Person executor)
    {
        Description = description;
        Deadline = deadline;
        Initiator = initiator;
        Executor = executor;
        Status = TaskStatus.Assigned;
        TaskReport = null;
    }

    // Взять в работу
    public void TakeInWork()
    {
        if (Status == TaskStatus.Assigned && Executor != null)
        {
            Status = TaskStatus.InProgress;
            Console.WriteLine(Executor.Name + " взял в работу: " + Description);
        }
    }

    // Делегировать другому
    public void DelegateTo(Person newExecutor)
    {
        if (Executor != null)
        {
            Console.WriteLine(Executor.Name + " делегировал задачу \"" + Description + "\" сотруднику " + newExecutor.Name);
            Executor = newExecutor;
            Status = TaskStatus.Assigned;
        }
    }

    // Отклонить задачу
    public void Reject()
    {
        if (Executor != null)
        {
            Console.WriteLine(Executor.Name + " отклонил задачу: " + Description);
            Executor = null;
            Status = TaskStatus.Assigned;
        }
    }

    // Переназначить после отклонения
    public void AssignTo(Person newExecutor)
    {
        Executor = newExecutor;
        Status = TaskStatus.Assigned;
        Console.WriteLine("Задача \"" + Description + "\" назначена сотруднику " + newExecutor.Name);
    }

    // Сдать отчёт
    public void CreateReport(string text)
    {
        if (Status == TaskStatus.InProgress && Executor != null)
        {
            TaskReport = new Report(text, Executor);
            Status = TaskStatus.UnderReview;
            Console.WriteLine(Executor.Name + " сдал отчёт по задаче: " + Description);
        }
    }

    // Утвердить отчёт
    public void ApproveReport()
    {
        if (Status == TaskStatus.UnderReview)
        {
            Status = TaskStatus.Completed;
            Console.WriteLine("Отчёт принят. Задача выполнена: " + Description);
        }
    }

    // Вернуть отчёт на доработку
    public void RejectReport()
    {
        if (Status == TaskStatus.UnderReview)
        {
            TaskReport = null;
            Status = TaskStatus.InProgress;
            Console.WriteLine("Отчёт возвращён на доработку: " + Description);
        }
    }
}

// Класс Проект
class Project
{
    public string Description;
    public DateTime Deadline;
    public Person Customer; // Заказчик
    public Person TeamLead; // Тимлид
    public List<ProjectTask> Tasks;
    public ProjectStatus Status;

    public Project(string description, DateTime deadline, Person customer, Person teamLead)
    {
        Description = description;
        Deadline = deadline;
        Customer = customer;
        TeamLead = teamLead;
        Tasks = new List<ProjectTask>();
        Status = ProjectStatus.Project;
    }

    // Создание задачи (только в статусе "Проект")
    public void AddTask(string description, DateTime deadline, Person executor)
    {
        if (Status == ProjectStatus.Project)
        {
            ProjectTask task = new ProjectTask(description, deadline, TeamLead, executor);
            Tasks.Add(task);
            Console.WriteLine("Создана задача \"" + description + "\" для " + executor.Name);
        }
        else
        {
            Console.WriteLine("Ошибка: создавать задачи можно только в статусе Проект!");
        }
    }

    // Удаление задачи
    public void RemoveTask(ProjectTask task)
    {
        Tasks.Remove(task);
        Console.WriteLine("Задача удалена: " + task.Description);
    }

    // Перевод проекта в статус "Исполнение"
    public void StartExecution()
    {
        if (Status == ProjectStatus.Project)
        {
            Status = ProjectStatus.Execution;
            Console.WriteLine("\nПроект переведен в статус Исполнение.\n");
        }
    }

    // Проверка и закрытие проекта
    public void CheckAndClose()
    {
        foreach (ProjectTask task in Tasks)
        {
            if (task.Status != TaskStatus.Completed)
            {
                Console.WriteLine("Проект еще не завершен.");
                return;
            }
        }

        Status = ProjectStatus.Closed;
        Console.WriteLine("\nВсе задачи выполнены! Проект переведен в статус Закрыт.");
    }
}

class Program
{
    static void Main()
    {
        // Заказчик и тимлид
        Person customer = new Person("Заказчик");
        Person teamLead = new Person("Тимлид Алексей");

        // Команда из 10 человек
        List<Person> team = new List<Person>();
        team.Add(new Person("Сотрудник 1"));
        team.Add(new Person("Сотрудник 2"));
        team.Add(new Person("Сотрудник 3"));
        team.Add(new Person("Сотрудник 4"));
        team.Add(new Person("Сотрудник 5"));
        team.Add(new Person("Сотрудник 6"));
        team.Add(new Person("Сотрудник 7"));
        team.Add(new Person("Сотрудник 8"));
        team.Add(new Person("Сотрудник 9"));
        team.Add(new Person("Сотрудник 10"));

        // Создаем 1 проект
        Project project = new Project("Создание сайта", DateTime.Now.AddDays(30), customer, teamLead);

        // Назначаем минимум 1 задачу каждому из 10 человек
        for (int i = 0; i < team.Count; i++)
        {
            project.AddTask("Задача " + (i + 1), DateTime.Now.AddDays(10), team[i]);
        }

        // Переводим проект в статус "Исполнение"
        project.StartExecution();

        // Задача 1: делегирование
        project.Tasks[0].DelegateTo(team[1]);
        project.Tasks[0].TakeInWork();
        project.Tasks[0].CreateReport("Сделано");
        project.Tasks[0].ApproveReport();

        // Задача 2: отклонение и переназначение
        project.Tasks[1].Reject();
        project.Tasks[1].AssignTo(team[2]);
        project.Tasks[1].TakeInWork();
        project.Tasks[1].CreateReport("Готово");
        project.Tasks[1].ApproveReport();

        // Задача 3: возврат отчёта на доработку
        project.Tasks[2].TakeInWork();
        project.Tasks[2].CreateReport("Черновик");
        project.Tasks[2].RejectReport();
        project.Tasks[2].CreateReport("Финальная версия");
        project.Tasks[2].ApproveReport();

        // Задачи 4–10: обычное выполнение
        for (int i = 3; i < project.Tasks.Count; i++)
        {
            project.Tasks[i].TakeInWork();
            project.Tasks[i].CreateReport("Отчет по задаче " + (i + 1));
            project.Tasks[i].ApproveReport();
        }

        // Закрываем проект
        project.CheckAndClose();

        Console.ReadLine();
    }
}

