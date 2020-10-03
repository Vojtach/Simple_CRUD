using System;
using System.IO;
using System.Text;
using System.Threading;

public static class ui
{
    public static void UserTyping()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Data.userInput = Console.ReadLine();
        Console.ResetColor();
    }
    public static void ErrorMsg(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(msg);
        Console.ResetColor();
    }
    public static void SuccesMsg(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(msg);
        Console.ResetColor();
    }
    public static void SystemMsg(string msg)
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine(msg);
        Console.ResetColor();
    }
    public static void HintMsg(string msg)
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine(msg);
        Console.ResetColor();
    }
}
public class Person
{

    public string Name { get; set; }
    public string Location { get; set; }
    public string Class { get; set; }
    public int Id { get; set; }

    public Person(string name, string location, string classValue, int id)
    {

        Location = location;
        Name = name;
        Class = classValue;
        Id = id;
    }

    public void ShowPerson()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("New Person:\n");
        Console.WriteLine("Name = {0} | Location = {1} | Class = {2} | ID = {3}", Name, Location, Class, Id);
        Console.ResetColor();
    }

    public static void MakePerson()
    {
        string[] _PersonTemplate = new string[3];

        ui.SystemMsg("Add persons name:");
        ui.UserTyping();
        _PersonTemplate[0] = Data.userInput;
        ui.SystemMsg("Add persons location:");
        ui.UserTyping();
        _PersonTemplate[1] = Data.userInput;
        ui.SystemMsg("Add person class:");
        ui.UserTyping();
        _PersonTemplate[2] = Data.userInput;
        ui.SystemMsg("Assigned ID: " + Data.id);

        do
        {
            db.GetID();

            Person newPerson = new Person(_PersonTemplate[0], _PersonTemplate[1], _PersonTemplate[2], Data.id);

            newPerson.ShowPerson();

            StreamWriter vystupData = new StreamWriter(Data._cestaVystupData, true, Encoding.UTF8);
            vystupData.WriteLine("{0};{1};{2};{3}", Data.id, newPerson.Name, newPerson.Location, newPerson.Class);
            Data.id++;
            vystupData.Close();

            StreamWriter vystupID = new StreamWriter(Data._cestaData, false);
            vystupID.WriteLine("{0}", Data.id);
            vystupID.Close();

            Console.ForegroundColor = ConsoleColor.Green;
            ui.SuccesMsg("Person was succesfully created!");
            Console.ResetColor();

            ui.HintMsg("Type 'stop' to stop generating new persons");
            ui.UserTyping();

        } while (Data.userInput != "stop");
    }
}

public static class db
{
    public static void GetID()
    {
        StreamReader getID = new StreamReader(Data._cestaData);
        int.TryParse(getID.ReadLine(), out Data.id);
        getID.Close();
    }
    public static void MakeDB()
    {
        StreamWriter vystupData = new StreamWriter(Data._cestaVystupData, false, Encoding.UTF8);
        vystupData.WriteLine("START DB");
        vystupData.Close();

        StreamWriter vystupID = new StreamWriter(Data._cestaData, false);
        vystupID.WriteLine("{0}", Data.id);
        vystupID.Close();
    }

    public static void GetFileContent()
    {
        try
        {
            StreamReader search = new StreamReader(Data._cestaVystupData);
            while (search.Peek() >= 0)
            {
                Console.WriteLine(search.ReadLine());
            }
            search.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("The process failed: {0}", e.ToString());
        }
    }
}

public static class Data
{
    public static int id = 0;
    public static string _cestaVystupData = "../../../log/DB.csv";
    public static string _cestaData = "../../../log/Data.txt";
    public static string userInput = "";

    public static void SaveData()
    {
        StreamWriter saveData = new StreamWriter(_cestaData, false, Encoding.UTF8);
        saveData.WriteLine("{0}\n{1}", id, _cestaData);
        saveData.Close();
    }

    public class Menu
    {

        static void Main()
        {
            do
            {
                Console.Clear();
                Data.userInput = "";
                Console.WriteLine("**** Main Menu ****");
                ui.HintMsg(" New Person [create]\n Make Database [make-db]\n Show data [show]\n Type [end] to leave");
                Console.WriteLine("*********************");
                ui.UserTyping();

                switch (Data.userInput)
                {
                    case "show":
                        ui.SystemMsg("ID - Name - Location - Class");
                        db.GetFileContent();
                        Console.ReadKey();
                        break;
                    case "create":
                        try
                        {
                            Data.userInput = "";
                            ui.SystemMsg("Starting person generator...");
                            Person.MakePerson();
                        }
                        catch (System.Exception)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            ui.ErrorMsg("Database ready!");
                            Console.ResetColor();
                            Thread.Sleep(3000);
                        }
                        break;
                    case "make-db":
                        Data.userInput = "";
                        ui.HintMsg("Create new database? (Data WILL be lost) [y/n]");
                        ui.UserTyping();
                        if (Data.userInput == "n")
                        {
                            ui.SystemMsg("OK...");
                            Thread.Sleep(1000);
                            break;
                        }
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        ui.SystemMsg("Creating DB...");
                        Thread.Sleep(1000);
                        Console.ResetColor();
                        db.MakeDB();

                        break;
                }
            } while (Data.userInput != "end");
            Data.SaveData();

        }
    }
}
