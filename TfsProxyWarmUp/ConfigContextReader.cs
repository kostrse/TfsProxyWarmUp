using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using TfsProxyWarmUp.Config;
using TfsProxyWarmUp.WarmUp;

namespace TfsProxyWarmUp
{
    public static class ConfigContextReader
    {
        public static List<WarmUpContext> Read()
        {
            WarmUpConfigurationSection warmUpConfiguration = (WarmUpConfigurationSection)ConfigurationManager.GetSection("warmUp");

            if (warmUpConfiguration == null)
                throw new Exception("Configuration section 'warmUp' not defined in configuration file.");

            List<WarmUpContext> contexts = new List<WarmUpContext>();

            foreach (ProjectCollectionElement projectCollection in warmUpConfiguration.ProjectCollections)
            {
                List<string> itemSpecs = new List<string>();

                foreach (ItemSpecElement itemSpec in projectCollection.ItemSpecs)
                {
                    itemSpecs.Add(itemSpec.ServerPath);
                }

                contexts.Add(new WarmUpContext(projectCollection.CollectionUrl, warmUpConfiguration.ProxyUrl, itemSpecs));
            }

            return contexts;
        }
    }
}
