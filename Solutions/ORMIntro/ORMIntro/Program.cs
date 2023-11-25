using Microsoft.EntityFrameworkCore;
using ORMIntro;

ApplicationDBContext db = new ApplicationDBContext();

var towns = db.Towns.Include(t => t.Country);

foreach (var town in towns)
{
    Console.WriteLine(town.Name + $" is in {town.Country?.Name}");
}