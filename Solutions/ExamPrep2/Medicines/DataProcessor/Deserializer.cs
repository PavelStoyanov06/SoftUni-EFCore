namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ImportDtos;
    using Medicines.Extensions;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data!";
        private const string SuccessfullyImportedPharmacy = "Successfully imported pharmacy - {0} with {1} medicines.";
        private const string SuccessfullyImportedPatient = "Successfully imported patient - {0} with {1} medicines.";

        public static string ImportPatients(MedicinesContext context, string jsonString)
        {
            List<ImportPatientDto> patientsDto = jsonString.DeserializeFromJson<List<ImportPatientDto>>();

            List<Patient> patients = new List<Patient>();

            StringBuilder sb = new StringBuilder();

            foreach (var patientDto in patientsDto)
            {
                if (!IsValid(patientDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var patientToAdd = new Patient()
                {
                    AgeGroup = patientDto.AgeGroup,
                    FullName = patientDto.FullName,
                    Gender = patientDto.Gender,
                };

                List<int> medicineIds = new List<int>();

                foreach (var medicineId in patientDto.Medicines)
                {
                    if (!medicineIds.Contains(medicineId) && context.Medicines.Select(m => m.Id).Contains(medicineId))
                    {
                        patientToAdd.PatientsMedicines.Add(new PatientMedicine()
                        {
                            MedicineId = medicineId
                        });

                        medicineIds.Add(medicineId);
                    }
                    else sb.AppendLine(ErrorMessage);
                }

                patients.Add(patientToAdd);
                sb.AppendLine(String.Format(SuccessfullyImportedPatient, patientToAdd.FullName, patientToAdd.PatientsMedicines.Count));
            }

            context.Patients.AddRange(patients);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPharmacies(MedicinesContext context, string xmlString)
        {
            List<ImportPharmacyDto> pharmaciesDto = xmlString.DeserializeFromXml<List<ImportPharmacyDto>>("Pharmacies");

            List<Pharmacy> pharmacies = new List<Pharmacy>();

            StringBuilder sb = new StringBuilder();

            foreach (var pharmacyDto in pharmaciesDto)
            {
                if (!IsValid(pharmacyDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var pharmacyToAdd = new Pharmacy()
                {
                    Name = pharmacyDto.Name,
                    PhoneNumber = pharmacyDto.PhoneNumber,
                    IsNonStop = pharmacyDto.IsNonStop == "true" ? true : false
                };

                foreach (var medicine in pharmacyDto.Medicines)
                {
                    if (!IsValid(medicine))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var failedDate = DateTime.ParseExact("0001-01-01", "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    var medicineProductionDate = DateTime.ParseExact(medicine.ProductionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    var medicineExpiryDate = DateTime.ParseExact(medicine.ExpiryDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    if (medicineProductionDate == failedDate || medicineExpiryDate == failedDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (medicineProductionDate >= medicineExpiryDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (pharmacyToAdd.Medicines.Any(m => m.Name == medicine.Name && m.Producer == medicine.Producer))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var medicineToAdd = new Medicine()
                    {
                        Name = medicine.Name,
                        Category = (Category)medicine.Category,
                        ExpiryDate = medicineProductionDate,
                        ProductionDate = medicineExpiryDate,
                        Price = medicine.Price,
                        Producer = medicine.Producer,
                        PharmacyId = pharmacyToAdd.Id
                    };

                    pharmacyToAdd.Medicines.Add(medicineToAdd);
                }

                pharmacies.Add(pharmacyToAdd);
                sb.AppendLine(String.Format(SuccessfullyImportedPharmacy, pharmacyToAdd.Name, pharmacyToAdd.Medicines.Count));
            }

            context.Pharmacies.AddRange(pharmacies);
            context.SaveChanges();

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
