using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using TfsProxyWarmUp.WarmUp;

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

            // Reading settings (given via command-line or configuration file)

            List<WarmUpContext> contexts;

            try
            {
                if (options.UseConfig)
                {
                    // Taking parameters from configuration file

                    contexts = ConfigContextReader.Read();
                }
                else
                {
                    // Taking parameters from command-line

                    contexts = CommandLineContextReader.Read(options);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Application settings are incorrect.");
                Console.WriteLine(ex.Message);

                Environment.ExitCode = 2;
                return;
            }

            // Performing warm up

            try
            {
                _logger.Info("Starting warm up...");

                foreach (var context in contexts)
                {
                    new ProjectCollectionWarmUp(context).Run();
                }

                _logger.Info("Done.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, string.Format("The program failed with error: {0}", ex.Message));
                Environment.ExitCode = 1;
            }
        }
    }
}
