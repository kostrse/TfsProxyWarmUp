using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TfsProxyWarmUp.WarmUp;

namespace TfsProxyWarmUp
{
    public static class CommandLineContextReader
    {
        public static List<WarmUpContext> Read(Options options)
        {
            if (options.ItemSpecs.Count == 0)
                throw new Exception("At least one ItemSpec should be specified.");

            List<WarmUpContext> contexts = new List<WarmUpContext>();
            contexts.Add(new WarmUpContext(new Uri(options.ProjectCollectionUrl), new Uri(options.ProxyUrl), options.ItemSpecs));

            return contexts;
        }
    }
}
