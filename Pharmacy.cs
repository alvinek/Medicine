using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Medicine
{
    public class Pharmacy
    {
        DbFiles _dbFiles;
        UserDb _userDb;
        MedicineDb _mainDb;
        public Pharmacy(DbFiles dbFiles)
        {
            _dbFiles = dbFiles;
            _userDb = new UserDb(File.ReadAllLines(_dbFiles.UserDbFile).ToList());
            _mainDb = new MedicineDb(File.ReadAllLines(_dbFiles.MainDbFile).ToList());
        }

        public int Start()
        {
            CheckFileExists(_dbFiles.MainDbFile);
            CheckFileExists(_dbFiles.UserDbFile);

            if (LogIn())
            {
                RenderWelcome();
                MainMenuInputCatch();
            }
            else
            {
                ConsoleGUI.Render("Nie udało się zalogować, kończymy aplikacje");
                return (int)Program.ExitCode.RunError;
            }

            return (int)Program.ExitCode.Success;
        }

        void CheckFileExists(string file)
        {
            if (!File.Exists(file))
            {
                throw new FileNotFoundException("Nie odnaleziono pliku " + file);
            }
        }

        bool LogIn()
        {
            while (true)
            {
                var userName = ConsoleGUI.PromptRender("Nazwa użytkownika: ");
                var userPassword = ConsoleGUI.PromptRender("Hasło użytkownika: ");

                if (!_userDb.UserExists(userName, userPassword))
                {
                    ConsoleGUI.Render("Niepoprawna nazwa uzytkownika lub hasło, spróbować ponownie?");
                    if (ConsoleGUI.PromptRender("Wpisz y jeśli tak lub n jeśli nie") == "y")
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                _userDb.SetCurrentUser(userName);
                return true;
            }
        }

        void RenderWelcome()
        {
            List<string> strings = new List<string>();

            strings.Add($"Witaj {_userDb.GetCurrentUserName()}!");

            strings.Add(string.Empty);
            strings.Add("1. Przeglądaj listę leków");
            strings.Add("2. Dodaj lek");
            strings.Add("3. Usuń lek");
            strings.Add("4. Szukaj lek");
            strings.Add("5. Pokaż użytkowników");
            strings.Add("6. Dodaj użytkownika");
            strings.Add("7. Usuń użytkownika");
            strings.Add("8. Wyloguj się");
            strings.Add(string.Empty);

            ConsoleGUI.Render(strings);
        }

        void MainMenuInputCatch()
        {
            string mainMenuInput = ConsoleGUI.PromptRender("Wybór: ");
            if (char.IsDigit(mainMenuInput[0]))
            {
                switch (int.Parse(mainMenuInput[0].ToString() /* co by nikt nie żartował, że wpisze 10 */ ))
                {
                    case 1:
                        DbBrowser.Browse(ref _mainDb);
                        break;
                    case 2:
                        _mainDb.AddMedicine();
                        break;
                    case 3:
                        _mainDb.RemoveMedicine();
                        break;
                    case 4:
                        _mainDb.SearchMedicine();
                        break;
                    case 5:
                        _userDb.ShowUsers();
                        break;
                    case 6:
                        _userDb.AddUser();
                        break;
                    case 7:
                        _userDb.DeleteUser();
                        break;
                    case 8:
                        _userDb.LogoutUser();
                        if (!LogIn()) Environment.Exit((int)Program.ExitCode.Success);
                        break;
                }
                RenderWelcome();
                MainMenuInputCatch();
            }
            else
            {
                ConsoleGUI.ErrorRender("Niepoprawny wybór, naciśnij Enter by spróbować ponownie");
                Console.ReadLine();
                RenderWelcome();
                MainMenuInputCatch();
            }
        }
    }
}
