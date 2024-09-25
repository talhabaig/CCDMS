using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCDMSServices.DTO
{
    public class ResponseDTO
    {
        public ResponseDTO()
        {
            Status = true;
            Message = "Success";
           
        }
        public bool Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
