using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TfsProxyWarmUp.WarmUp
{
    public class WarmUpContext
    {
        public Uri ProjectCollectionUrl { get; private set; }
        public Uri ProxyUrl { get; private set; }

        public List<string> ItemSpecs { get; private set; }
        
        public WarmUpContext(Uri projectCollectionUrl, Uri proxyUrl, List<string> itemSpecs)
        {
            if (projectCollectionUrl == null)
                throw new ArgumentNullException("projectCollectionUrl");

            if (proxyUrl == null)
                throw new ArgumentNullException("proxyUrl");

            if (itemSpecs == null)
                throw new ArgumentNullException("itemSpecs");

            ProjectCollectionUrl = projectCollectionUrl;
            ProxyUrl = proxyUrl;

            ItemSpecs = itemSpecs;
        }
    }
}
