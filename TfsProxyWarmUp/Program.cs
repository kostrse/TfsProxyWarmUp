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
            var options = new Options();

            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                var warmUp = new ProjectCollectionWarmUp(options.ProjectCollectionUrl, options.ProxyUrl, new[] { options.ItemSpec });
                warmUp.Run();
            }
        }
    }
}
