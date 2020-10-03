using System;
using System.IO;
using System.Text;
using System.Threading;

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
}

public class Osoba
{

	public string Jmeno { get; set; }
	public string Lokace { get; set; }
	public string Trida { get; set; }
	public int Id { get; set; }

	public Osoba(string jmeno, string lokace, string trida, int id)
	{

		Lokace = lokace;
		Jmeno = jmeno;
		Trida = trida;
		Id = id;
	}

	public void ShowPerson()
	{
		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.WriteLine("Vytvořený uživatel:\n");
		Console.WriteLine("Jméno = {0} Lokace = {1} Třída = {2} ID = {3}", Jmeno, Lokace, Trida, Id);
		Console.ResetColor();
	}
}

public class Sablona
{
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
	static void UserTyping()
	{
		Console.ForegroundColor = ConsoleColor.Yellow;
		Data.userInput = Console.ReadLine();
		Console.ResetColor();
	}
	static void ErrorMsg(string msg)
	{
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine(msg);
		Console.ResetColor();
	}
	static void SuccesMsg(string msg)
	{
		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine(msg);
		Console.ResetColor();
	}
	static void SystemMsg(string msg)
	{
		Console.ForegroundColor = ConsoleColor.DarkCyan;
		Console.WriteLine(msg);
		Console.ResetColor();
	}
	static void HintMsg(string msg)
	{
		Console.ForegroundColor = ConsoleColor.DarkCyan;
		Console.WriteLine(msg);
		Console.ResetColor();
	}

	static string[] NewOsoba(int id)
	{
		string[] osoby = new string[3];

		SystemMsg("Zadej jméno nového uživatele");
		UserTyping();
		osoby[0] = Data.userInput;
		SystemMsg("Zadej lokaci nového uživatele");
		UserTyping();
		osoby[1] = Data.userInput;
		SystemMsg("Zadej třídu nového uživatele");
		UserTyping();
		osoby[2] = Data.userInput;
		SystemMsg("Přiřazené ID: " + id);

		return osoby;
	}

	static void VytvorOsobu()
	{
		do
		{
			GetID();

			string[] _OsobaTemplate = NewOsoba(Data.id);
			Osoba osoba = new Osoba(_OsobaTemplate[0], _OsobaTemplate[1], _OsobaTemplate[2], Data.id);

			osoba.ShowPerson();

			StreamWriter vystupData = new StreamWriter(Data._cestaVystupData, true, Encoding.UTF8);
			vystupData.WriteLine("{0};{1};{2};{3}", Data.id, osoba.Jmeno, osoba.Lokace, osoba.Trida);
			Data.id++;
			vystupData.Close();

			StreamWriter vystupID = new StreamWriter(Data._cestaData, false);
			vystupID.WriteLine("{0}", Data.id);
			vystupID.Close();

			Console.ForegroundColor = ConsoleColor.Green;
			SuccesMsg("Osoba úspěšně zapsána!");
			Console.ResetColor();

			HintMsg("Napiš 'stop' pro ukončení");
			UserTyping();

		} while (Data.userInput != "stop");


	}

	static void GetID()
	{
		StreamReader getID = new StreamReader(Data._cestaData);
		int.TryParse(getID.ReadLine(), out Data.id);
		getID.Close();
	}

	static void VytvorDB()
	{
		StreamWriter vystupData = new StreamWriter(Data._cestaVystupData, false, Encoding.UTF8);
		vystupData.WriteLine("START DB");
		vystupData.Close();

		StreamWriter vystupID = new StreamWriter(Data._cestaData, false);
		vystupID.WriteLine("{0}", Data.id);
		vystupID.Close();
	}

	public class Menu
	{

		static void Main()
		{
			do
			{
				Console.Clear();
				Data.userInput = "";
				Console.WriteLine("**** Hlavní Menu ****");
				HintMsg(" Nová osoba [zapis]\n Vytvoř DB [makedb]\n Show data [show]\n Type [end] to leave");
				Console.WriteLine("*********************");
				UserTyping();

				switch (Data.userInput)
				{
					case "show":
						SystemMsg("ID - Name - Location - Class");
						GetFileContent();
						Console.ReadKey();
						break;
					case "zapis":
						try
						{
							Data.userInput = "";
							SystemMsg("Zahajování zápisu osob...");
							VytvorOsobu();
						}
						catch (System.Exception)
						{
							Console.ForegroundColor = ConsoleColor.Red;
							ErrorMsg("Databáze není vytvořena!");
							Console.ResetColor();
							Thread.Sleep(3000);
						}
						break;
					case "makedb":
						Data.userInput = "";
						HintMsg("Opravdu vytvořit novou databázi? (Dojde ke ztrátě dat) [y/n]");
						UserTyping();
						if (Data.userInput == "n")
						{
							SystemMsg("OK...");
							Thread.Sleep(1000);
							break;
						}
						Console.ForegroundColor = ConsoleColor.Cyan;
						SystemMsg("Vytváření DB...");
						Thread.Sleep(1000);
						Console.ResetColor();
						VytvorDB();

						break;
				}
			} while (Data.userInput != "end");
			Data.SaveData();

		}
	}
}
