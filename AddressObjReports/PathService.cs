﻿namespace AddressObjReports
{
    class PathService
    {
        /// <summary>
        /// Краткое описание функции
        /// </summary>
        /// <returns>Что возвращает функция</returns>
        public static string SelectPath()
        {
            Console.WriteLine("Для сохранения в корневую папку программы нажмите Enter");
            Console.WriteLine("Пример: C:\\example");
            Console.Write("Введите полный путь для сохранения отчета: ");
            string? path = Console.ReadLine();
            return string.IsNullOrEmpty(path) ? "report.docx" : path + "\\report.docx";
        }
    }
}