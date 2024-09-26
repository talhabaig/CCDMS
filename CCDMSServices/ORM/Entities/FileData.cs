using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCDMSServices.ORM.Entities
{
    public class FileData
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("File")]
        public long FileId { get; set; }
#nullable disable
        public Files File { get; set; }
        public long Hours { get; set; }
        public decimal Mean { get; set; }
        public decimal Median { get; set; }
        public decimal Std { get; set; }
        public decimal Count { get; set; }
    }
}
