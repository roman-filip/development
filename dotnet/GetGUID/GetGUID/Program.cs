using System;
using System.IO;
using TLI;

namespace GetGUID
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length != 1)
            {
                ShowHelp();
                return;
            }

            var dllFile = args[0];
            if (!File.Exists(dllFile))
            {
                Console.WriteLine("Dll file '{0}' doesn't exist.", dllFile);
                return;
            }

            GetGuid(dllFile);
        }

        private static void ShowHelp()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("    GetGUID.exe dll_file");
        }

        private static void GetGuid(string dllFile)
        {
            var tliApplication = new TLIApplicationClass();
            Console.WriteLine(tliApplication.TypeLibInfoFromFile(dllFile).GUID);
        }
    }
}
