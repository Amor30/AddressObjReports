using System.Text;

namespace AddressObjReports
{
    /// <summary>
    /// Класс, который реализует интерфейс ICreateReport.
    /// </summary>
    public class CreateReportToTxt : ICreateReport
    {
        /// <summary>
        /// Создает отчет в формате txt.
        /// </summary>
        /// <param name="groupedAddresses">Словарь с ключом LevelName со значением списка адресов.</param>
        /// <returns></returns>
        public async Task CreateReport(Dictionary<string, List<Address>> groupedAddresses)
        {
            Console.Clear();
            var path = PathService.SelectPath() + ".txt";
            var text = CreateFormatText(groupedAddresses);

            using (StreamWriter writer = new StreamWriter(path, false))
            {
                await writer.WriteLineAsync(text);
            }
        }

        private string CreateFormatText(Dictionary<string, List<Address>> groupedAddresses)
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

        private string GetReportDate()
        {
            using var sr = new StreamReader("gar_delta_xml\\version.txt");
            return sr.ReadLine() ?? throw new InvalidOperationException();
        }
    }
}
