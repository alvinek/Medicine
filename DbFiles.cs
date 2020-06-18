using System;
using System.IO;

namespace Medicine
{
    public class DbFiles
    {
        string mainDbFile = Path.Combine(Environment.CurrentDirectory, "db.txt");
        string userDbFile = Path.Combine(Environment.CurrentDirectory, "dbuser.txt");

        public string MainDbFile
        {
            get { return mainDbFile; }
            set { throw new NotImplementedException(); }
        }

        public string UserDbFile
        {
            get { return userDbFile; }
            set { throw new NotImplementedException(); }
        }
    }
}
