using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace TfsProxyWarmUp
{
    class Program
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

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

            try
            {
                _logger.Info("Starting warm up...");

                var warmUp = new ProjectCollectionWarmUp(options.ProjectCollectionUrl, options.ProxyUrl, options.ItemSpecs);
                warmUp.Run();

                _logger.Info("Done.");
            }
            catch (Exception ex)
            {
                _logger.ErrorException(string.Format("The program failed with error: {0}", ex.Message), ex);
                Environment.ExitCode = 1;
            }

            Console.ReadKey();
        }
    }
}
