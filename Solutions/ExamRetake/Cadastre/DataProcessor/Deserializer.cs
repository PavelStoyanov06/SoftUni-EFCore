namespace Cadastre.DataProcessor
{
    using Cadastre.Data;
    using Cadastre.Data.Enumerations;
    using Cadastre.Data.Models;
    using Cadastre.DataProcessor.ImportDtos;
    using Cadastre.Extensions;
    using Newtonsoft.Json.Linq;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid Data!";
        private const string SuccessfullyImportedDistrict =
            "Successfully imported district - {0} with {1} properties.";
        private const string SuccessfullyImportedCitizen =
            "Succefully imported citizen - {0} {1} with {2} properties.";

        public static string ImportDistricts(CadastreContext dbContext, string xmlDocument)
        {
            List<ImportDistrictDto> districtDtos = xmlDocument.DeserializeFromXml<List<ImportDistrictDto>>("Districts");

            List<District> districts = new List<District>();

            var sb = new StringBuilder();

            foreach (var districtDto in districtDtos)
            {
                if (!IsValid(districtDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if(dbContext.Districts.Select(d => d.Name).Contains(districtDto.Name))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var districtToAdd = new District() 
                { 
                    Name = districtDto.Name,
                    PostalCode = districtDto.PostalCode,
                    Region = (Region) Enum.Parse(typeof(Region), districtDto.Region)
                };

                foreach (var property in districtDto.Properties)
                {
                    if (!IsValid(property))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime failedDate = DateTime.ParseExact("01/01/0001", "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    DateTime propDateTime = DateTime.ParseExact(property.DateOfAcquisition, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    if (propDateTime == failedDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if(districtToAdd.Properties.Select(p => p.PropertyIdentifier).Contains(property.PropertyIdentifier))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if(districtToAdd.Properties.Select(p => p.Address).Contains(property.Address))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var propertyToAdd = new Property()
                    {
                        PropertyIdentifier = property.PropertyIdentifier,
                        Address = property.Address,
                        Area = property.Area,
                        Details = property.Details,
                        DateOfAcquisition = propDateTime
                    };

                    districtToAdd.Properties.Add(propertyToAdd);
                }
                
                districts.Add( districtToAdd );
                sb.AppendLine(String.Format(SuccessfullyImportedDistrict, districtToAdd.Name, districtToAdd.Properties.Count));
            }

            dbContext.AddRange(districts);
            dbContext.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCitizens(CadastreContext dbContext, string jsonDocument)
        {
            List<ImportCitizenDto> citizenDtos = jsonDocument.DeserializeFromJson<List<ImportCitizenDto>>();

            List<Citizen> citizens = new List<Citizen>();

            StringBuilder sb = new StringBuilder();

            foreach (var citizen in citizenDtos)
            {
                if (!IsValid(citizen))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime failedDate = DateTime.ParseExact("01-01-0001", "dd-MM-yyyy", CultureInfo.InvariantCulture);
                var citizenDate = DateTime.ParseExact(citizen.BirthDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                if(citizenDate ==  failedDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var citizenToAdd = new Citizen()
                {
                    FirstName = citizen.FirstName,
                    LastName = citizen.LastName,
                    BirthDate = citizenDate,
                    MaritalStatus = (MaritalStatus)Enum.Parse(typeof(MaritalStatus), citizen.MaritalStatus)
                };

                foreach (var property in citizen.Properties)
                {
                    if(!dbContext.Properties.Select(p => p.Id).Contains(property))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    citizenToAdd.PropertiesCitizens.Add(new PropertyCitizen()
                    {
                        PropertyId = property,
                        CitizenId = citizenToAdd.Id
                    });
                }

                citizens.Add(citizenToAdd);
                sb.AppendLine(String.Format(SuccessfullyImportedCitizen, citizenToAdd.FirstName, citizenToAdd.LastName, citizenToAdd.PropertiesCitizens.Count));
            }

            dbContext.Citizens.AddRange(citizens);
            dbContext.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
