using System.IO.Compression;
using System.Net;
using System.Text.Json;

namespace AddressObjReports
{
    /// <summary>
    /// Класс, который представляет сервис по скачиванию файлов.
    /// </summary>
    public class DownloadFileService
    {
        /// <summary>
        /// Скачивает zip файл по ссылке.
        /// </summary>
        /// <param name="url">Ссылка на JSON с тегом GarXMLDeltaURL, где хранится zip.</param>
        /// <param name="zipName">Название zip файла.</param>
        /// <param name="directoryName">Название каталога, куда распакуется zip.</param>
        /// <returns></returns>
        public async Task DownloadFileToBaseDirectory(string url, string zipName, string directoryName)
        {
            Console.WriteLine("Загрузка zip файла");
            using (var file = new WebClient())
            {
                file.DownloadFile(await GetLastDownloadFileInfo(url), zipName);
            }
            Console.Clear();
            Console.WriteLine("Распаковка zip файла");
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
