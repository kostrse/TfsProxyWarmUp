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
        [Option('c', "collection", Required = true, HelpText = "TFS project collection URL.")]
        public string ProjectCollectionUrl { get; set; }

        [Option('p', "proxy", Required = true, HelpText = "TFS Proxy server URL.")]
        public string ProxyUrl { get; set; }

        [ValueList(typeof(List<string>))]
        public List<string> ItemSpecs { get; set; }

        [HelpOption]
        public string Usage()
        {
            var usage = HelpText.AutoBuild(this);

            usage.AddPreOptionsLine("Usage:");
            usage.AddPreOptionsLine("tfsproxywarmup -c=collectionUrl -p=proxyUrl itemSpec1 [itemSpec2 ...]");

            usage.AddPostOptionsLine("Example:");
            usage.AddPostOptionsLine("tfsproxywarmup -c=http://tfsserver:8080/tfs/DefaultCollection -p=http://tfsproxy:8081/ $/Project1/MAIN $/Project1/RELEASE/1.1");
            usage.AddPostOptionsLine(string.Empty);
            usage.AddPostOptionsLine(string.Empty);

            return usage;
        }
    }
}
