using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace TfsProxyWarmUp.Config
{
    [ConfigurationCollection(typeof(ProjectCollectionElement), AddItemName = "projectCollection", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class ProjectCollectionsElement : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ProjectCollectionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as ProjectCollectionElement).CollectionUrl;
        }
    }
}
