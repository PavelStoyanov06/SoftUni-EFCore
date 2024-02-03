using Microsoft.EntityFrameworkCore;
using SoftUni;
using SoftUni.Data;
using SoftUni.Models;
using System.Xml;

public class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        using SoftUniContext context = new SoftUniContext();
    }
}