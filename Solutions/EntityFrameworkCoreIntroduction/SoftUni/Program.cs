using SoftUni;
using SoftUni.Data;

public class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        using SoftUniContext context = new SoftUniContext();

        StartUp.GetEmployeesFromResearchAndDevelopment(context);
    }
}