using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.VersionControl.Client;
using NLog;
using TfsProxyWarmUp.Tfs;

namespace TfsProxyWarmUp.WarmUp
{
    public class ProjectCollectionWarmUp
    {
        private const int MAX_MINUTES_FOR_GETITEMS = 10;

        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        private readonly WarmUpContext _context;
        private readonly HashSet<int> _alreadyDownloadedFileIds = new HashSet<int>();

        public ProjectCollectionWarmUp(WarmUpContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            _context = context;
        }

        public void Run()
        {
            _logger.Info("Starting for Project Collection: {0}", _context.ProjectCollectionUrl);

            try
            {
                DateTime startTime = DateTime.UtcNow;

                // Setting TFS Proxy URL environment variable on the process level

                _logger.Info("Configuring connection via TFS Proxy: {0}", _context.ProxyUrl);

                Environment.SetEnvironmentVariable("TFSPROXY", _context.ProxyUrl.AbsoluteUri);

                // Initializing connection to TFS Project Collection by given URL

                _logger.Info("Connecting to TFS...");

                TfsTeamProjectCollection projectCollection = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(_context.ProjectCollectionUrl);

                projectCollection.Connect(ConnectOptions.IncludeServices);

                VersionControlServer versionControl = projectCollection.GetService<VersionControlServer>();

                // Processing ItemSpecs

                foreach (string itemSpec in _context.ItemSpecs)
                {
                    RunForItemSpec(versionControl, itemSpec);
                }

                // Calculating elapsed time

                DateTime finishTime = DateTime.UtcNow;
                TimeSpan elapsedTime = finishTime - startTime;

                _logger.Info("Warm up for the Project Collection done. Elapsed time: {0}", elapsedTime);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, string.Format("Warm up for the Project Collection failed with error: {0}", ex.Message));
            }
        }

        private void RunForItemSpec(VersionControlServer versionControl, string itemSpec)
        {
            _logger.Info(string.Format("Starting for ItemSpec: {0}", itemSpec));

            try
            {
                DateTime startTime = DateTime.UtcNow;

                // Retrieving list of items

                _logger.Info("Retrieving list of items...");

                var items = versionControl.GetItems(itemSpec, VersionSpec.Latest, RecursionType.Full, DeletedState.NonDeleted, ItemType.File, true);

                DateTime finishTimeGetItems = DateTime.UtcNow;
                TimeSpan elapsedTimeGetItems = finishTimeGetItems - startTime;

                _logger.Info("Retrieved {0} items to warm up. Retrieval time: {1}", items.Items.Length, elapsedTimeGetItems);

                if (elapsedTimeGetItems > TimeSpan.FromMinutes(MAX_MINUTES_FOR_GETITEMS))
                {
                    _logger.Warn("Retrieval of items for the ItemSpec took more than {0} minutes, actual time: {1}. Consider reconfiguration of ItemSpecs for the Project Collection.",
                        MAX_MINUTES_FOR_GETITEMS, elapsedTimeGetItems);
                }

                // Processing retrieved items

                _logger.Info("Downloading items...");

                foreach (var item in items.Items)
                {
                    try
                    {
                        // Extracting file ID from the item's download URL to avoid downloading of already warmed up files
                        // during this session

                        int fileId = item.GetFileId();

                        if (!_alreadyDownloadedFileIds.Contains(fileId))
                        {
                            // Requesting file and closing stream, because we don't need the file itself

                            item.DownloadFile().Dispose();

                            _logger.Debug("Downloaded: {0}:{1}", item.ServerItem, item.ChangesetId);

                            // Marking the file as downloaded during this session

                            _alreadyDownloadedFileIds.Add(fileId);
                        }
                        else
                            _logger.Debug("Skipped (already downloaded): {0}:{1}", item.ServerItem, item.ChangesetId);
                    }
                    catch (Exception ex)
                    {
                        _logger.Warn(ex, string.Format("Failed to process item: {0}:{1}", item.ServerItem, item.ChangesetId));
                    }
                }

                // Calculating elapsed time

                DateTime finishTime = DateTime.UtcNow;
                TimeSpan elapsedTime = finishTime - startTime;

                _logger.Info("Warm up for the ItemSpec done. Elapsed time: {0}", elapsedTime);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, string.Format("Warm up for the ItemSpec failed with error: {0}", ex.Message));
            }
        }
    }
}
