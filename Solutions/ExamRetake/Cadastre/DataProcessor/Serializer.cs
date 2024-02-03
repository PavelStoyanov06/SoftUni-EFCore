using Cadastre.Data;
using Cadastre.DataProcessor.ExportDtos;
using Cadastre.Extensions;
using System.Globalization;

namespace Cadastre.DataProcessor
{
    public class Serializer
    {
        public static string ExportPropertiesWithOwners(CadastreContext dbContext)
        {
            DateTime afterDate = DateTime.ParseExact("01/01/2000", "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var properties = dbContext.Properties
                .Where(p => p.DateOfAcquisition >  afterDate)
                .OrderByDescending(p => p.DateOfAcquisition)
                .ThenBy(p => p.PropertyIdentifier)
                .Select(p => new ExportPropertyDto()
                {
                    PropertyIdentifier = p.PropertyIdentifier,
                    Area = p.Area,
                    Address = p.Address,
                    DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Owners = p.PropertiesCitizens
                    .Select(p => p.Citizen)
                    .Select(o => new ExportOwnerDto()
                    {
                        LastName = o.LastName,
                        MaritalStatus = o.MaritalStatus.ToString()
                    })
                    .OrderBy(o => o.LastName)
                    .ToArray()
                })
                .ToArray();

            return properties.SerializeToJson();
        }

        public static string ExportFilteredPropertiesWithDistrict(CadastreContext dbContext)
        {
            var properties = dbContext.Properties
                .Where(p => p.Area >= 100)
                .OrderByDescending(p => p.Area)
                .ThenBy(p => p.DateOfAcquisition)
                .Select(p => new ExportPropertyAreaDto()
                {
                    PropertyIdentifier = p.PropertyIdentifier,
                    Area = p.Area,
                    DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy"),
                    PostalCode = p.District.PostalCode
                })
                .ToArray();

            return properties.SerializeToXml("Properties");
        }
    }
}
