namespace AddressObjReports
{
    /// <summary>
    /// Интерфейс который определяет реализацию создания отчета.
    /// </summary>
    public interface ICreateReport
    {
        public Task CreateReport(Dictionary<string, List<Address>> groupedAddresses);
    }
}
