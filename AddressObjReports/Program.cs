using System;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Xml;
using System.Net.Http;
using System.Text.Json;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace AddressObjReports
{
    internal class Program
    {
        //возвращает информацию о последней версии файлов, доступных для скачивания(тип DownloadFileInfo)
        static readonly string url = "https://fias.nalog.ru/WebServices/Public/GetLastDownloadFileInfo";

        static readonly string zipName = "gar_delta_xml.zip";
        static readonly string directoryName = "gar_delta_xml";

        public static async Task Main(string[] args)
        {
            var downloadFileService = new DownloadFileService();
            await downloadFileService.DownloadFileToBaseDirectory(url, zipName, directoryName);
            try
            {
                var addressesService = new AddressesService();
                var groupAddresses = addressesService.GetGroupAddresses(directoryName);
                GenerateReport(new CreateReportToTxt(), groupAddresses);
            }
            catch
            {
                Console.WriteLine("Непредвиденная ошибка");
            }
            finally
            {
                File.Delete(zipName);
                Directory.Delete(directoryName, true);
            }
        }

        private static void GenerateReport(ICreateReport createReport, Dictionary<string, List<Address>> groupedAddresses)
        {
            createReport.CreateReport(groupedAddresses);
        }
    }
}
