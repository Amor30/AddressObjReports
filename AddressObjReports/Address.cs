namespace AddressObjReports
{
    /// <summary>
    /// Класс, который представляет адрес.
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Уровень адресного объекта.
        /// </summary>
        public required int Level { get; set; }
        /// <summary>
        /// Название уровня.  
        /// </summary>
        public required string LevelName { get; set; }
        /// <summary>
        /// Краткое наименование типа объекта.
        /// </summary>
        public required string TypeName { get; set; }
        /// <summary>
        /// Наименование объекта.
        /// </summary>
        public required string Name { get; set; }
    }
}