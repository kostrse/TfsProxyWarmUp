using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace TfsProxyWarmUp.Config
{
    public class WarmUpConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("proxyUrl", IsRequired = true)]
        public Uri ProxyUrl
        {
            get
            {
                return (Uri)base["proxyUrl"];
            }
        }

        [ConfigurationProperty("projectCollections")]
        public ProjectCollectionsElement ProjectCollections
        {
            get
            {
                return (ProjectCollectionsElement)base["projectCollections"];
            }
        }
    }
}
