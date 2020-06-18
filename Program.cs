using Medicine.Database;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Medicine
{
    class Program
    {
        public enum ExitCode : int
        {
            Success = 0,
            InitError = 1,
            RunError = 2
        }

        static int Main(string[] args)
        {
            DbFiles db = new DbFiles();

            if (args.Any())
            {
                switch (args[0].ToLower())
                {
                    case "--reset":
                        Reset(db);
                        break;
                }
            }

            Pharmacy pharmacy = new Pharmacy(db);


            return pharmacy.Start();
        }

        static void Reset(DbFiles db)
        {
            File.Delete(db.MainDbFile);
            File.Delete(db.UserDbFile);

            List<string> mainDbFileDefault = new List<string>
            {
                "1,Auriga Flavo-C Forte Serum,15,ml,Auriga,false,0,30.95",
                "2,VITA BUER D3,120,kaps,VITA,false,0,40.30",
                "4,PALMERS Krem łagodzący do twarzy,100,ml,Palmers,false,0,40.34",
                "5,BEROTEC aerozol inhalacyjny,200,dawka,BEROTEC,true,20,60.45",
                "6,CORONAL 10 mg,30,tabl,CORONAL,true,20,10.20"
            };

            List<string> userDbFileDefault = new List<string>
            {
                "1,Jan,janek,Kowalski,kowalski1",
                "2,Piotr,piotr3,Dąb,piotrek",
                "3,Anna,ania,Najdek,aneta"
            };

            File.WriteAllLines(db.MainDbFile, mainDbFileDefault);
            File.WriteAllLines(db.UserDbFile, userDbFileDefault);
        }
    }
}
