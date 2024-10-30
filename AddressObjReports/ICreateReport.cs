namespace AddressObjReports
{
    public interface ICreateReport
    {
        public Task CreateReport(Dictionary<string, List<Address>> groupedAddresses);
    }
}
