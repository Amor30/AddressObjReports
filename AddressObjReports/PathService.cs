namespace AddressObjReports
{
    class PathService
    {
        /// <summary>
        /// Этот метод записывает консольный ввод в переменную path.
        /// </summary>
        /// <returns>Возвращает путь, куда сохранится отчет.</returns>
        public static string SelectPath()
        {
            Console.WriteLine("Для сохранения в корневую папку программы нажмите Enter");
            Console.WriteLine("Пример: C:\\example");
            Console.Write("Введите полный путь для сохранения отчета: ");
            string? path = Console.ReadLine();
            return string.IsNullOrEmpty(path) ? "report" : path + "\\report";
        }
    }
}
