using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AddressObjReports
{
    public class DownloadFileService
    {
        public async Task DownloadFileToBaseDirectory(string url, string zipName, string directoryName)
        {
            using (var file = new WebClient())
            {
                file.DownloadFile(await GetLastDownloadFileInfo(url), zipName);
            }
            ZipFile.ExtractToDirectory(zipName,directoryName);
        }

        private async Task<string> GetLastDownloadFileInfo(string url)
        {
            using var client = new HttpClient();
            using HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();
            var test = JsonSerializer.Deserialize<DownloadFileInfo>(json) ?? throw new InvalidOperationException();
            return test.GarXMLDeltaURL;
        }

    }
}
