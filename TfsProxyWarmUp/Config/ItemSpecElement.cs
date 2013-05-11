using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace TfsProxyWarmUp.Config
{
    public class ItemSpecElement : ConfigurationElement
    {
        [ConfigurationProperty("serverPath", IsKey = true, IsRequired = true)]
        public string ServerPath
        {
            get
            {
                return (string)base["serverPath"];
            }
        }
    }
}
