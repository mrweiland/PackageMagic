

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageMagic.PackageManager
{



    public static class Utils
    {
        public static bool ExtendedLogging { get; private set; }

        public static void LogMessages(string message, bool extendedLogging = false)
        {
            ExtendedLogging = false;
            if (ExtendedLogging)
            {
                if (extendedLogging)
                {
                    Console.WriteLine("**********Extended logging**********");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(message);

                    Console.WriteLine("**********End Extended logging**********");

                    return;
                }
            }
            if (extendedLogging == false)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(message);
            }
        }
    }
}
