namespace AddressObjReports
{
    public class Program
    {
        //возвращает информацию о последней версии файлов, доступных для скачивания(тип DownloadFileInfo)
        static readonly string url = "https://fias.nalog.ru/WebServices/Public/GetLastDownloadFileInfo";
        static readonly string zipName = "gar_delta_xml.zip";
        static readonly string directoryName = "gar_delta_xml";

        static async Task Main(string[] args)
        {
            try
            {
                var downloadFileService = new DownloadFileService();
                await downloadFileService.DownloadFileToBaseDirectory(url, zipName, directoryName);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Не удалось установить SSL соединение");
                return;
            }
            catch
            {
                Console.WriteLine("Непредвиденная ошибка");
                return;
            }

            try
            {
                var addressesService = new AddressesService();
                var groupAddresses = addressesService.GetGroupAddresses(directoryName);
                //GenerateReport(new CreateReportToTxt(), groupAddresses);
                GenerateReport(new CreateReportToDocx(), groupAddresses);
            }
            catch
            {
                Console.WriteLine("Непредвиденная ошибка");
                return;
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
