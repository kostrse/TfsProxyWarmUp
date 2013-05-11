using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace TfsProxyWarmUp
{
    public static class TfsItemExtensions
    {
        public static int GetFileId(this Item item)
        {
            // Verifying state of the item

            if (item == null)
                throw new ArgumentNullException("item");

            if (item.ItemType != ItemType.File)
                throw new InvalidOperationException("The item should be of type ItemType.File.");

            string downloadUrl = item.DownloadUrl;

            if (string.IsNullOrEmpty(downloadUrl))
                throw new InvalidOperationException("The item should have DownloadUrl initialized.");

            // Extracting FileId from DownloadUrl

            int fileId;

            const string fid = "&fid=";

            int startIndex;

            if (downloadUrl.Length >= fid.Length - 1 && string.CompareOrdinal(downloadUrl, 0, fid, 1, fid.Length - 1) == 0)
            {
                // If FID parameter found in the beginning of the query string

                startIndex = fid.Length - 1;
            }
            else
            {
                // Trying to find FID in the middle of the string

                int fidStartIndex = downloadUrl.IndexOf(fid, StringComparison.Ordinal);

                if (fidStartIndex < 0)
                    throw new InvalidOperationException("DownloadUrl doesn't contain FileId.");

                startIndex = fidStartIndex + fid.Length;
            }

            if (startIndex == downloadUrl.Length)
                throw new InvalidOperationException("DownloadUrl doesn't contain FileId.");

            int endIndex = downloadUrl.IndexOf('&', startIndex);

            if (endIndex < 0)
                endIndex = downloadUrl.Length;

            string fileIdStr = downloadUrl.Substring(startIndex, endIndex - startIndex);
            
            if (string.IsNullOrEmpty(fileIdStr) || !int.TryParse(fileIdStr, out fileId))
                throw new InvalidOperationException("DownloadUrl doesn't contain FileId.");

            return fileId;
        }
    }
}
