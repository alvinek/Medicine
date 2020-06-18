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
            if (args.Any())
            {
                switch (args[0].ToLower())
                {
                    case "--reset":
                        Reset();
                        break;
                }
            }

            DbFiles db = new DbFiles();
            Pharmacy pharmacy = new Pharmacy(db);

            return pharmacy.Start();
        }

        static void Reset()
        {

        }
    }
}
