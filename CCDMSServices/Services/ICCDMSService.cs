using CCDMSServices.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCDMSServices.Services
{
    public interface ICCDMSService
    {
        Task AddFileDataAsync(IFormFile file);
        Task<ResponseDTO> GetFileData(FileFilterDTO? filters);
    }
}
