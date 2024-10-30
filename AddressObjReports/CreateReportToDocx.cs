using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Words.NET;
using Xceed.Document.NET;
using System.Drawing;
using System.Net;
using System.Reflection.Metadata;

namespace AddressObjReports
{
    public class CreateReportToDocx : ICreateReport
    {
        public Task CreateReport(Dictionary<string, List<Address>> groupedAddresses)
        {
            string path = "report.docx";
            DocX report = DocX.Create(path);
            report.InsertParagraph("Отчет по добавленным адресным объектам за " + GetReportDate())
                .FontSize(20)
                .Color(Color.Coral)
                .Alignment = Alignment.center;
            foreach (var address in groupedAddresses)
            {
                report.InsertParagraph(address.Key)
                    .FontSize(16)
                    .Color(Color.Coral);
                report.InsertParagraph().InsertTableAfterSelf(CreateTable(report, address.Value));
            }
            report.Save();
            return Task.CompletedTask;
        }

        private static Table CreateTable(DocX report, List<Address> addresses)
        {
            Table table = report.AddTable(addresses.Count + 1, 2);
            table.Design = TableDesign.TableGrid;
            table.Rows[0].Cells[0].Paragraphs[0]
                .Append("Тип объекта")
                .FontSize(14)
                .Bold(true);
            table.Rows[0].Cells[1].Paragraphs[0]
                .Append("Наименование")
                .FontSize(14)
                .Bold(true);
            int i = 1;
            foreach (Address address in addresses)
            {
                table.Rows[i].Cells[0].Paragraphs[0]
                    .Append(address.TypeName)
                    .FontSize(14);
                table.Rows[i].Cells[1].Paragraphs[0]
                    .Append(address.Name)
                    .FontSize(14);
                i++;
            }
            return table;
        }

        private static string GetReportDate()
        {
            using var sr = new StreamReader("gar_delta_xml\\version.txt");
            return sr.ReadLine() ?? throw new InvalidOperationException();
        }
    }
}
