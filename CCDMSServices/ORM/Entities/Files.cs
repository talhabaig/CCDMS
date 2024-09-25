using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCDMSServices.ORM.Entities
{
    public class Files
    {
        [Key]
        public long Id { get; set; }
        public string CountryName { get; set; }
        public string FarmName { get; set; }
        public string CoopNumber { get; set; }
        public DateTime GrowthStartDate { get; set; }
        public long DataCollectionTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<FileData> Data {  get; set; }


    }
}
