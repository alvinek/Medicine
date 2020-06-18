using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Medicine
{
    public class User
    {
        public int Id { get; set; }
        public string UserNickname { get; set; }
        public string Name { get; set; }
        public string Subname { get; set; }
        public string Password { get; set; }

        public User(int userId, string userName, string userNickname, string userSubname, string userPassword)
        {
            Id = userId;
            Name = userName;
            Subname = userSubname;
            Password = userPassword;
            UserNickname = userNickname;
        }
    }
    public class UserDb
    {
        List<User> users = new List<User>();
        User currentUser;
        public UserDb(List<string> db)
        {
            foreach (var line in db)
            {
                if (line.StartsWith("#")) continue;
                var split = line.Split(',');

                User user = new User(int.Parse(split[0]), split[1], split[2], split[3], split[4]);

                users.Add(user);
            }
        }

        public bool UserExists(string user, string password)
        {
            var userFromDb = users.Where(x => x.UserNickname.Equals(user));
            if (userFromDb.Count() < 1)
                return false;
            if (userFromDb.Count() > 1)
                throw new Exception("W bazie jest dwoch uzytkownikow o tej samej nazwie!");
            if (userFromDb.First().Password.Equals(password))
                return true;

            return false;
        }

        public void SetCurrentUser(string nickName)
        {
            currentUser = users.Where(x => x.UserNickname.Equals(nickName)).First();
        }

        public string GetCurrentUserName() => currentUser.Name;

        public void ShowUsers(bool wait = true)
        {
            List<string> toRender = new List<string>();

            foreach (var user in users)
            {
                toRender.Add("\tID:" + user.Id + "\tImię:" + user.Name + "\tNazwisko: " + user.Subname + "\tNick: " + user.UserNickname);
            }

            ConsoleGUI.Render(toRender);
            if (wait) ConsoleGUI.PromptRender("Dowolny przycisk opuści ten widok");
        }

        public void DeleteUser()
        {
            ShowUsers(false);

            string userInput = ConsoleGUI.PromptRender("Podaj ID użytkownika, którego chcesz usunąć: ");
            if (int.TryParse(userInput, out int input))
            {
                if (currentUser.Id == input)
                {
                    ConsoleGUI.ErrorRender("Nie możesz usunąć sam siebie", true);
                    return;
                }

                User user = GetUser(input);
                if (user != null)
                {
                    users.Remove(user);
                    Flush();
                }
                else
                {
                    ConsoleGUI.ErrorRender("Nie ma takiego użytkownika", true);
                    return;
                }
            }

        }

        public User GetUser(int id)
        {
            if (users.Any(x => x.Id == id))
            {
                return users.First(x => x.Id == id);
            }

            return null;
        }

        public void AddUser()
        {
            var userName = ConsoleGUI.PromptRender("Imie uzytkownika: ").Replace(",", "").Trim();
            var userSurname = ConsoleGUI.PromptRender("Nazwisko uzytkownika: ").Replace(",", "").Trim();
            string userNickname;
            while (true)
            {
                userNickname = ConsoleGUI.PromptRender("Nick: ").Replace(",", "").Trim();
                if (users.Any(x => x.UserNickname.Equals(userNickname)))
                {
                    ConsoleGUI.ErrorRender("Ta nazwa użytkownika jest już zajęta, spróbuj inną");
                }
                else break;
                continue;
            }
            var userPassword = ConsoleGUI.PromptRender("Hasło użytkownika: ").Replace(",", "").Trim();

            int id = users.Max(x => x.Id) + 1;

            users.Add(new User(id, userName, userNickname, userSurname, userPassword));

            Flush();
        }

        public void LogoutUser() => currentUser = null;

        private void Flush()
        {
            DbFiles dbFiles = new DbFiles();
            File.Delete(dbFiles.UserDbFile);
            List<string> newFile = new List<string>();

            foreach (var user in users)
            {
                newFile.Add($"{user.Id},"
                    + $"{user.Name},"
                    + $"{user.UserNickname},"
                    + $"{user.Subname},"
                    + $"{user.Password}");
            }

            File.WriteAllLines(dbFiles.UserDbFile, newFile);
        }
    }
}
