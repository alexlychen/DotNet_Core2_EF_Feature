using AutoLotDAL_Core2.DataInitialization;
using AutoLotDAL_Core2.EF;
using AutoLotDAL_Core2.Models;
using AutoLotDAL_Core2.Repos;
using System;

namespace AutoLotDAL_Core2.TestDriver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Function with ADO.NET EF Core 2 ****\n");

            using (var context = new AutoLotContext())
            {
                MyDataInitializer.RecreateDatabase(context);
                MyDataInitializer.InitializeData(context);
                foreach (Inventory c in context.Cars)
                {
                    Console.WriteLine(c);
                }

                //Dependency Injection
                Console.WriteLine("Using a Repository\n");
                using (var repo = new InventoryRepo(context))
                {
                    foreach (Inventory c in repo.GetAll())
                    {
                        Console.WriteLine(c);
                    }
                }
                //Console.ReadLine();
            }
            
            //fallback
            Console.WriteLine("Using a Repository\n");
            using (var repo = new InventoryRepo())
            {
                foreach (Inventory c in repo.GetAll())
                {
                    Console.WriteLine(c);
                }
            }
            Console.ReadLine();
        }
    }
}
