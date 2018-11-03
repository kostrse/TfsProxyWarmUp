using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace TfsProxyWarmUp
{
    public class Options
    {
        [Option("useconfig", HelpText = "Run warm up using configuration from file.", MutuallyExclusiveSet = "сonfig")]
        public bool UseConfig { get; set; }

        [Option('c', "collection", HelpText = "TFS project collection URL (if not useconfig).", MutuallyExclusiveSet = "cmd")]
        public string ProjectCollectionUrl { get; set; }

        [Option('p', "proxy", HelpText = "TFS Proxy server URL (if not useconfig).", MutuallyExclusiveSet = "cmd")]
        public string ProxyUrl { get; set; }

        [ValueList(typeof(List<string>))]
        public List<string> ItemSpecs { get; set; }

        [HelpOption]
        public string Usage()
        {
            var usage = HelpText.AutoBuild(this);

            usage.AddPreOptionsLine("Usage:");
            usage.AddPreOptionsLine("tfsproxywarmup --useconfig");
            usage.AddPreOptionsLine("tfsproxywarmup -c collectionUrl -p proxyUrl itemSpec1 [itemSpec2 ...]");

            usage.AddPostOptionsLine("Example:");
            usage.AddPostOptionsLine("tfsproxywarmup -c http://tfsserver:8080/tfs/DefaultCollection -p http://tfsproxy:8081/ $/Project1/MAIN $/Project1/RELEASE/1.1");
            usage.AddPostOptionsLine(string.Empty);
            usage.AddPostOptionsLine(string.Empty);

            return usage;
        }
    }
}
