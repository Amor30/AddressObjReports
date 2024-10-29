using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressObjReports
{
    public class CreateReportToTxt : ICreateReport
    {
        public async Task CreateReport(Dictionary<string, List<Address>> groupedAddresses)
        {
            string path = "report.txt";
            string text = CreateFormatText(groupedAddresses);

            using (StreamWriter writer = new StreamWriter(path, false))
            {
                await writer.WriteLineAsync(text);
            }
        }

        private static string CreateFormatText(Dictionary<string, List<Address>> groupedAddresses)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("Отчет по добавленным адресным объектам за " + GetReportDate());

            foreach (var address in groupedAddresses)
            {
                stringBuilder.Append($"\n\n\n\n{address.Key}\n\n");
                stringBuilder.Append("Тип объекта".PadRight(15));
                stringBuilder.Append("Наименование\n");
                foreach (var test in address.Value)
                {
                    stringBuilder.Append($"{test.TypeName}".PadRight(15));
                    stringBuilder.Append($"{test.Name}\n");
                }
            }
            return stringBuilder.ToString();
        }

        private static string GetReportDate()
        {
            using var sr = new StreamReader("gar_delta_xml\\version.txt");
            return sr.ReadLine() ?? throw new InvalidOperationException();
        }
    }
}
