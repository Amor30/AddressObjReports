using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressObjReports
{
    public interface ICreateReport
    {
        public Task CreateReport(Dictionary<string, List<Address>> groupedAddresses);
    }
}
