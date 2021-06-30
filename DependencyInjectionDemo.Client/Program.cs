
using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using DependencyInjectionDemo.DataAccess;
using System.Data;

namespace DependencyInjectionDemo.Client
{

    class Program
    {
        private static bool _isAlive = true;
        private static List<MenuItem> _menuItems = new List<MenuItem>();
        private static DataProcessor _dataProcessor;


        static void Main(string[] args)
        {

            var dataAccess = ResolveDataAccess();
            _dataProcessor = new DataProcessor(dataAccess);
            _dataProcessor.GetPeopleList();
            LoadMenuItems();

            while (_isAlive)
            {
                try
                {
                    DisplayMenuItems();
                    int selection = Int16.Parse(Console.ReadLine());
                    var menuItem = _menuItems[selection - 1];
                    menuItem.Command.Invoke();
                }
                catch (ArgumentOutOfRangeException)
                {
                    System.Console.WriteLine("Invalid number selected. Try again");
                }
                catch (FormatException)
                {
                    System.Console.WriteLine("Invalid entry. Try again");
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                    System.Console.WriteLine("Process terminated with error");
                    System.Console.WriteLine("Press any key to end");
                    System.Console.ReadLine();
                    _isAlive = false;
                }
            }
        }

        private static IDataAccess ResolveDataAccess()
        {
            var container = ContainerConfig.Configure();

            return container.Resolve<IDataAccess>();
        }



        private static void LoadMenuItems()
        {
            _menuItems.Add(new MenuItem() { Title = "Display People List", Command = DisplayPeopleList });
            _menuItems.Add(new MenuItem() { Title = "Add Person", Command = ProcessAddPerson });
            _menuItems.Add(new MenuItem() { Title = "Remove Person", Command = ProcessRemovePerson });
            _menuItems.Add(new MenuItem() { Title = "Update Person", Command = ProcessUpdatePerson });
            _menuItems.Add(new MenuItem() { Title = "Exit", Command = ExitApp });
        }

        private static void DisplayMenuItems()
        {
            System.Console.WriteLine();
            for (int i = 1; i <= _menuItems.Count; i++)
                System.Console.WriteLine($"({i}) {_menuItems.ElementAt(i - 1).Title}");
        }

        private static void DisplayPeopleList()
        {
            _dataProcessor.GetPeopleList();
            foreach (var person in _dataProcessor.People)
                System.Console.WriteLine($"{person.Id} {person.FullName}");
        }

        private static void ProcessAddPerson()
        {
            var firstName = (string)ProcessUserInput("Enter First Name", typeof(string));
            var lastName = (string)ProcessUserInput("Enter Last Name", typeof(string));
            _dataProcessor.AddPerson(firstName, lastName);
            System.Console.WriteLine("Person added successfully");
        }

        private static void ProcessRemovePerson()
        {

            DisplayPeopleList();
            var id = (Int16)ProcessUserInput("Enter Id of person to remove", typeof(Int16));

            var person = _dataProcessor.People.SingleOrDefault(p => p.Id == id);
            if (person != null)
            {
                _dataProcessor.RemovePerson(person);
                System.Console.WriteLine("Person removed successfully");

            }
            else
                System.Console.WriteLine($"Id: {id} not found in DB");
        }

        private static void ProcessUpdatePerson()
        {
            DisplayPeopleList();
            var id = (Int16)ProcessUserInput("Enter Id of person to update", typeof(Int16));
            var person = _dataProcessor.People.SingleOrDefault(p => p.Id == id);

            if (person != null)
            {
                if ((bool)ProcessUserInput($"Update First Name ({person.FirstName})?", typeof(bool)))
                    person.FirstName = (string)ProcessUserInput("Enter new First Name", typeof(string));
                if ((bool)ProcessUserInput($"Update Last Name ({person.LastName})?", typeof(bool)))
                    person.LastName = (string)ProcessUserInput("Enter new Last Name", typeof(string));

                _dataProcessor.UpdatePerson(person);
                System.Console.WriteLine("Person updated successfully");
            }
            else
                System.Console.WriteLine("Person not found in DB");
        }

        private static object ProcessUserInput(string displayMessage, Type type)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            System.Console.Write($"{displayMessage}: ");
            Console.ForegroundColor = ConsoleColor.White;

            if (type == typeof(Int16) || type == typeof(Int32))
                return Int16.Parse(Console.ReadLine());
            if (type == typeof(bool))
                return Console.ReadLine().ToUpper() == "Y";

            return Console.ReadLine();

        }


        private static void ExitApp()
        {
            _isAlive = false;
        }
    }
}
