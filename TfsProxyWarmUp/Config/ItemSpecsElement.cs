using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfsProxyWarmUp.Config
{
    [ConfigurationCollection(typeof(ItemSpecElement), AddItemName = "itemSpec", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class ItemSpecsElement : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ItemSpecElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as ItemSpecElement).ServerPath;
        }
    }
}
