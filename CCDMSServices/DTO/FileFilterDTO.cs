using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCDMSServices.DTO
{
    public class FileFilterDTO
    {
        public string? CountryName { get; set; }
        public string? FarmName { get; set; }
        public string? CoopNumber { get; set; }
        public string? GrowthStartDate { get; set; }
        public long? DataCollectionTime { get; set; }
    }
}
