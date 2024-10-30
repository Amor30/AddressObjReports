namespace AddressObjReports
{
    /// <summary>
    /// �����, ������� ������������ �����.
    /// </summary>
    public class Address
    {
        /// <summary>
        /// ������� ��������� �������.
        /// </summary>
        public required int Level { get; set; }
        /// <summary>
        /// �������� ������.  
        /// </summary>
        public required string LevelName { get; set; }
        /// <summary>
        /// ������� ������������ ���� �������.
        /// </summary>
        public required string TypeName { get; set; }
        /// <summary>
        /// ������������ �������.
        /// </summary>
        public required string Name { get; set; }
    }
}