using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TfsProxyWarmUp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Reading command-line arguments

            var options = new Options();

            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                Environment.ExitCode = 2;
                return;
            }
            else if (options.ItemSpecs.Count == 0)
            {
                Console.WriteLine(options.Usage());

                Console.WriteLine("You should specify at least one ItemSpec.");
                Console.WriteLine();
                
                Environment.ExitCode = 2;
                return;
            }

            // Performing warm up

            var warmUp = new ProjectCollectionWarmUp(options.ProjectCollectionUrl, options.ProxyUrl, options.ItemSpecs);
            warmUp.Run();
        }
    }
}
