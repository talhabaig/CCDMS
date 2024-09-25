using CCDMSServices.DTO;
using CCDMSServices.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CCDMS_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CSVReadController : ControllerBase
    {
        private readonly ICCDMSService cCDMSService;
        public CSVReadController(ICCDMSService cCDMSService)
        {
            this.cCDMSService = cCDMSService;
        }
        [HttpPost]
        public async Task<ResponseDTO> AddFile([Required][FromForm] FIleDTO File)
        {
            ResponseDTO responseDTO = new ResponseDTO();
            try
            {
                await cCDMSService.AddFileDataAsync(File.File);
                responseDTO.Status = true;
                responseDTO.Message = "File Data has been added successfully";
            }
            catch (Exception ex)
            {
                responseDTO.Status = false;
                responseDTO.Message = ex.Message;
            }
            return responseDTO;
        }
        [HttpGet]
        public async Task<ResponseDTO> GetFileData([FromQuery]FileFilterDTO? filters)
        {
            try
            {
                return await cCDMSService.GetFileData(filters);
            }
            catch (Exception ex)
            {
                return new ResponseDTO()
                {
                    Status = false,
                    Message = ex.Message
                };
            }
        }
    }
}
