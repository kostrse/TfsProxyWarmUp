using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;

namespace TfsProxyWarmUp
{
    public class Options
    {
        [Option]
        public string ProjectCollectionUrl { get; set; }

        [Option]
        public string ProxyUrl { get; set; }

        [Option]
        public string ItemSpec { get; set; }
    }
}
