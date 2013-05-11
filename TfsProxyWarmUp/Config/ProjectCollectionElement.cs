using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace TfsProxyWarmUp.Config
{
    public class ProjectCollectionElement : ConfigurationElement
    {
        [ConfigurationProperty("collectionUrl", IsKey = true, IsRequired = true)]
        public Uri CollectionUrl
        {
            get
            {
                return (Uri)base["collectionUrl"];
            }
        }

        [ConfigurationProperty("itemSpecs")]
        public ItemSpecsElement ItemSpecs
        {
            get
            {
                return (ItemSpecsElement)base["itemSpecs"];
            }
        }
    }
}
