using CCDMSServices.ORM.Context;
using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CCDMSServices.ORM.Entities;
using System.Security.AccessControl;
using CCDMSServices.DTO;
using Microsoft.EntityFrameworkCore;

namespace CCDMSServices.Services
{
    public class CCDMSService : ICCDMSService
    {
        private readonly CCDMSDbContext db;
        public CCDMSService(CCDMSDbContext db)
        {
            this.db = db;
        }

        public async Task AddFileDataAsync(IFormFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (!(file.Length > 0))
            {
                throw new InvalidDataException("Empty File");
            }

            if (!IsCsvFile(file))
            {
                throw new InvalidDataException("File must me in csv format");
            }

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
            string[] parts = fileNameWithoutExtension.Split("___");
            if (parts.Length == 5)
            {
                string countryName = parts[0];
                string farmName = parts[1];
                string coopNumber = parts[2];
                string growthStartDate = parts[3];
                string longNumber = parts[4];

                DateTime growthDate;
                DateTime.TryParse(growthStartDate, out growthDate);

                var File = await db.Files
                      .Where(x => x.CountryName.ToLower() == countryName.ToLower() &&
                                  x.FarmName.ToLower() == farmName.ToLower() &&
                                  x.CoopNumber.ToLower() == coopNumber.ToLower() &&
                                  x.GrowthStartDate.Date == growthDate.Date &&
                                  x.DataCollectionTime.ToString() == longNumber
                      ).FirstOrDefaultAsync();

                if (File == null)
                {
                    File = new Files()
                    {
                        CoopNumber = coopNumber,
                        CountryName = countryName,
                        CreatedDate = DateTime.UtcNow,
                        DataCollectionTime = long.TryParse(longNumber, out long number) ? number : 0,
                        FarmName = farmName,
                        GrowthStartDate = DateTime.TryParse(growthStartDate, out DateTime date) ? date : default,
                    };

                    await db.Files.AddAsync(File);
                    await db.SaveChangesAsync();
                }


                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    var records = new List<FileData>();
                    csv.Read(); //To Ignore Headers
                    while (csv.Read())
                    {
                        var Hour = csv.GetField(0);
                        var Mean = csv.GetField(1);
                        var Median = csv.GetField(2);
                        var Std = csv.GetField(3);
                        var Count = csv.GetField(4);

                        records.Add(new FileData
                        {
                            Hours = int.TryParse(Hour, out int hour) ? hour : 0,
                            Mean = decimal.TryParse(Mean, out decimal mean) ? mean : 0,
                            Median = decimal.TryParse(Median, out decimal median) ? median : 0,
                            Std = decimal.TryParse(Std, out decimal std) ? std : 0,
                            Count = int.TryParse(Count, out int count) ? count : 0,
                            FileId = File.Id
                        });
                    }

                    await db.FileData.AddRangeAsync(records);
                    await db.SaveChangesAsync();
                }

            }
            else
            {
                throw new InvalidDataException("Invalid File Name");
            }

        }
        public async Task<ResponseDTO> GetFileData(FileFilterDTO? filters)
        {
            ResponseDTO responseDTO = new ResponseDTO();

            var data = db.Files
                         .Include(x => x.Data)
                         .Select(x => new
                         {
                             x.GrowthStartDate,
                             x.CreatedDate,
                             x.DataCollectionTime,
                             x.CoopNumber,
                             x.CountryName,
                             x.FarmName,
                             x.Id,
                             Data = x.Data.Select(y => new
                             {
                                 y.Count,
                                 y.Median,
                                 y.Mean,
                                 y.Hours,
                                 y.Std,
                                 y.Id
                             }).ToList()

                         }).AsQueryable();

            if (filters != null)
            {
                if (!string.IsNullOrEmpty(filters.CoopNumber))
                {
                    var oopNumberList = filters.CoopNumber.Split(',');
                    data = data.Where(x => oopNumberList.Contains(x.CoopNumber));
                }

                if (!string.IsNullOrEmpty(filters.FarmName))
                {
                    var farmNameList = filters.FarmName.Split(',');
                    data = data.Where(x => farmNameList.Contains(x.FarmName));
                }

                if (!string.IsNullOrEmpty(filters.CountryName))
                {
                    var countryNameList = filters.CountryName.Split(',');
                    data = data.Where(x => countryNameList.Contains(x.CountryName));
                }

                if (filters.DataCollectionTime.HasValue)
                {
                    data = data.Where(x => x.DataCollectionTime == filters.DataCollectionTime);
                }

                if (!string.IsNullOrEmpty(filters.GrowthStartDate))
                {
                    var splitDates = filters.GrowthStartDate.Split(',');

                    if (splitDates.Length == 1)
                    {
                        if (DateTime.TryParse(splitDates[0], out DateTime date))
                        {
                            data = data.Where(x => x.GrowthStartDate.Date == date.Date);
                        }
                    }
                    else if (splitDates.Length == 2)
                    {
                        if (DateTime.TryParse(splitDates[0], out DateTime date1) && DateTime.TryParse(splitDates[1], out DateTime date2))
                        {
                            if (date1 > date2)
                            {
                                (date1, date2) = (date2, date1);
                            }

                            data = data.Where(x => x.GrowthStartDate.Date >= date1.Date && x.GrowthStartDate.Date <= date2.Date);
                        }
                    }
                }
            }

            responseDTO.Data = await data.ToListAsync();
            return responseDTO;
        }
        private bool IsCsvFile(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (extension != ".csv")
            {
                return false;
            }

            var contentType = file.ContentType.ToLowerInvariant();
            if (contentType != "text/csv" && contentType != "application/csv" && contentType != "text/plain")
            {
                return false;
            }

            return true;
        }
    }
}
