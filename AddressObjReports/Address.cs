namespace AddressObjReports
{
    /// <summary>
    /// ��� �� �����
    /// </summary>
    public class Address
    {
        /// <summary>
        ///     ��� � ����� ��� ��������
        /// </summary>
        public required int Level { get; set; }
        public required string LevelName { get; set; }
        public required string TypeName { get; set; }
        public required string Name { get; set; }
    }
}