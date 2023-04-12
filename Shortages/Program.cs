
using Shortages.Data;
using Shortages.Models;

namespace Shortages;

public class Program
{
    private const string filePath = "Shortages.json";
    static void Main(string[] args)
    {

        IShortageData _iShortageData = new ShortageData(filePath);
        IUserData _iUserData = new UserData();

        while (true)
        {
            Console.WriteLine("Please, login");
            Console.Write("Username: ");
            var username = Console.ReadLine();
            Console.Write("Password: ");
            var password = Console.ReadLine();
            UserModel user = new();
            try
            {
                user = _iUserData.Login(username, password);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine($"Welcome, {user.Name}");
            Console.WriteLine("Enter a command: ");
            Console.WriteLine("r - register new shortage");
            Console.WriteLine("v - view shortages");
            Console.WriteLine("d - delete shortage");
            Console.WriteLine("e - exit");

            var command = Console.ReadLine();

            switch (command.ToLower())
            {
                case "r":
                    Console.WriteLine("Enter shortage information:");
                    Console.Write("Title: ");
                    var title = Console.ReadLine();
                    Console.WriteLine("Select room:");
                    Console.WriteLine("m - Meeting room");
                    Console.WriteLine("k - Kitchen");
                    Console.WriteLine("b - Bathroom");
                    string room = "";
                    var roomCommand = Console.ReadLine();
                    if (roomCommand == "m") room = "MeetingRoom";
                    else if (roomCommand == "k") room = "Kitchen";
                    else if (roomCommand == "b") room = "Bathroom";
                    Console.WriteLine("Select category:");
                    Console.WriteLine("e - electronics");
                    Console.WriteLine("f - food");
                    Console.WriteLine("o - other");
                    string category ="";
                    var categoryCommand = Console.ReadLine();
                    if (categoryCommand == "e") category = "Electronics";
                    else if (categoryCommand == "f") category = "Food";
                    else if (categoryCommand == "o") category = "Other";
                    
                    Console.Write("Priority (1-10): ");
                    var priority = int.Parse(Console.ReadLine());

                    var shortage = new ShortageModel
                    {
                        Title = title,
                        Name = user.Name,
                        Room = room,
                        Category = category,
                        Priority = priority,
                        CreatedOn = DateTime.Now,
                        UserId = user.Id
                    };

                    try
                    {
                        _iShortageData.AddShortage(shortage);
                        Console.WriteLine("Shortage registered successfully.");
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    break;

                case "d":
                    Console.Write("Enter shortage ID to delete: ");
                    var id = int.Parse(Console.ReadLine());

                    try
                    {
                        _iShortageData.DeleteShortage(id, user.Id, user.Type);
                        Console.WriteLine("Shortage deleted successfully.");
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    break;

                case "v":
                    var filter = new FilterModel();
                    Console.Write("Filter by title (leave empty for no filter): ");
                    filter.Title = Console.ReadLine();
                    Console.WriteLine("Filter by room (leave empty for no filter): ");
                    Console.WriteLine("m - Meeting room");
                    Console.WriteLine("k - Kitchen");
                    Console.WriteLine("b - Bathroom");
                    var roomCd = Console.ReadLine();
                    if (roomCd == "m") filter.Room = "MeetingRoom";
                    else if (roomCd == "k") filter.Room = "Kitchen";
                    else if (roomCd == "b") filter.Room = "Bathroom";
                    Console.WriteLine("Filter by category (leave empty for no filter): ");
                    Console.WriteLine("e - electronics");
                    Console.WriteLine("f - food");
                    Console.WriteLine("o - other");
                    var categoryCd = Console.ReadLine();
                    if (categoryCd == "e") filter.Category = "Electronics";
                    else if (categoryCd == "f") filter.Category = "Food";
                    else if (categoryCd == "o") filter.Category = "Other";
                    Console.Write("Filter by start date (yyyy-MM-dd, leave empty for no filter): ");
                    var startDateString = Console.ReadLine();
                    if (!string.IsNullOrEmpty(startDateString))
                    {
                        filter.StartDate = DateTime.Parse(startDateString);
                    }
                    Console.Write("FilterModel by end date (yyyy-MM-dd, leave empty for no filter): ");
                    var endDateString = Console.ReadLine();
                    if (!string.IsNullOrEmpty(endDateString))
                    {
                        filter.EndDate = DateTime.Parse(endDateString);
                    }

                    var shortages = _iShortageData.GetShortages(filter, user.Id, user.Type);

                    Console.WriteLine("{0,-5} {1,-20} {2,-25} {3,-15} {4,-15} {5,-10} {6,-20}", "ID", "Title", "Name", "Room", "Category", "Priority", "CreatedOn");

                    foreach (var sh in shortages)
                    {
                        Console.WriteLine("{0,-5} {1,-20} {2,-25} {3,-15} {4,-15} {5,-10} {6,-20}", sh.Id, sh.Title, sh.Name, sh.Room, sh.Category, sh.Priority, sh.CreatedOn);
                    }

                    break;

                case "e":
                    return;

                default:
                    Console.WriteLine("Invalid command.");
                break;
            }
            Console.WriteLine();
        }
    }
}

