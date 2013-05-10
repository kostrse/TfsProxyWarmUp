using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace TfsProxyWarmUp
{
    public class ProjectCollectionWarmUp
    {
        private readonly string _projectCollectionUrl;
        private readonly string _proxyUrl;

        private readonly List<string> _itemSpecs;

        private readonly HashSet<int> _alreadyDownloadedFileIds = new HashSet<int>();

        public ProjectCollectionWarmUp(string projectCollectionUrl, string proxyUrl, List<string> itemSpecs)
        {
            if (string.IsNullOrEmpty(projectCollectionUrl))
                throw new ArgumentNullException("projectCollectionUrl");

            if (string.IsNullOrEmpty(proxyUrl))
                throw new ArgumentNullException("proxyUrl");

            if (itemSpecs == null)
                throw new ArgumentNullException("itemSpecs");

            _projectCollectionUrl = projectCollectionUrl;
            _proxyUrl = proxyUrl;

            _itemSpecs = itemSpecs;
        }

        public void Run()
        {
            // Setting TFS Proxy URL environment variable on the process level

            Environment.SetEnvironmentVariable("TFSPROXY", _proxyUrl);

            // Initializing connection to TFS Project Collection by given URL

            var projectCollection = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(_projectCollectionUrl));

            projectCollection.Connect(ConnectOptions.IncludeServices);

            // Initializing VersionControlServer service and processing items

            var versionControl = projectCollection.GetService<VersionControlServer>();

            foreach (string itemSpec in _itemSpecs)
            {
                RunForItemSpec(versionControl, itemSpec);
            }
        }

        private void RunForItemSpec(VersionControlServer versionControl, string itemSpec)
        {
            var items = versionControl.GetItems(itemSpec, VersionSpec.Latest, RecursionType.Full, DeletedState.NonDeleted, ItemType.File, true);

            foreach (var item in items.Items)
            {
                // Extracting file ID from the item's download URL to avoid downloading of already warmed up files
                // during this session

                int fileId = item.GetFileId();

                if (!_alreadyDownloadedFileIds.Contains(fileId))
                {
                    // Requesting file and closing stream, because we don't need the file itself

                    item.DownloadFile().Dispose();

                    // Marking the file as downloaded during this session
                    
                    _alreadyDownloadedFileIds.Add(fileId);
                }
            }
        }
    }
}
