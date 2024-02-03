namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json.Linq;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Text.Json.Serialization;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data!";
        private const string SuccessfullyImportedPharmacy = "Successfully imported pharmacy - {0} with {1} medicines.";
        private const string SuccessfullyImportedPatient = "Successfully imported patient - {0} with {1} medicines.";

        public static string ImportPatients(MedicinesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            JArray patients = JArray.Parse(jsonString);
            foreach (var patient in patients)
            {
                try
                {
                    string fullName = patient.Value<string>("FullName");
                    int ageGroup = patient.Value<int>("AgeGroup");
                    int gender = patient.Value<int>("Gender");
                    int[] medicines = patient.Value<JArray>("Medicines").Values<int>().ToArray();

                    Patient newPatient = new Patient
                    {
                        FullName = fullName,
                        AgeGroup = (AgeGroup)ageGroup,
                        Gender = (Gender)gender,
                    };

                    foreach (var medicine in medicines)
                    {
                        PatientMedicine patientMedicine = new PatientMedicine { PatientId = newPatient.Id, MedicineId = medicine };
                        if (newPatient.PatientsMedicines.Contains(patientMedicine)) continue;
                        newPatient.PatientsMedicines.Add(patientMedicine);
                    }

                    context.Patients.Add(newPatient);

                    string result = string.Format(SuccessfullyImportedPatient, fullName, medicines.Length);
                    sb.AppendLine(result);
                }
                catch (Exception e)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportPharmacies(MedicinesContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Pharmacy));
            XmlSerializer serializerMeds = new XmlSerializer(typeof(Medicine));

            var pharmacies = XDocument.Parse(xmlString).Root.Elements();

            var sb = new StringBuilder();
            foreach (var pharmacy in pharmacies)
            {
                try
                {
                    bool shouldSkip = false;
                    var medicines = pharmacy.Element("Medicines").Elements("Medicine");
                    foreach (var medicine in medicines)
                    {
                        try
                        {
                            var medicineToAdd = (Medicine)serializerMeds.Deserialize(medicine.CreateReader());

                            if (medicines.Count(x => x == medicine) > 1)
                            {
                                continue;
                            }

                            DateTime productionDate = DateTime.Parse(medicineToAdd.ProductionDate.ToString(), CultureInfo.InvariantCulture);
                            DateTime expiryDate = DateTime.Parse(medicineToAdd.ExpiryDate.ToString(), CultureInfo.InvariantCulture);

                            if(productionDate >= expiryDate)
                            {
                                sb.AppendLine(ErrorMessage);
                                continue;
                            }
                        }
                        catch (Exception ex)
                        {
                            medicine.Remove();
                            sb.AppendLine(ErrorMessage);
                            continue;
                        }
                    }

                    if (shouldSkip)
                    {
                        shouldSkip = false;
                        continue;
                    }

                    var pharmacyToAdd = (Pharmacy)serializer.Deserialize(pharmacy.CreateReader());

                    context.Pharmacies.Add(pharmacyToAdd);

                    foreach (var medicine in pharmacyToAdd.Medicines)
                    {
                        if (pharmacyToAdd.Medicines.Count(x => x.Name == medicine.Name && x.Producer == medicine.Producer) > 1)
                        {
                            pharmacyToAdd.Medicines.Remove(medicine);
                            Console.WriteLine($"Removed: {medicine.Name}");
                        }
                    }

                    Console.WriteLine(String.Join($"{Environment.NewLine}", pharmacyToAdd.Medicines.Select(x => new { x.Name, x.Producer })));

                    Console.WriteLine();

                    sb.AppendLine(string.Format(SuccessfullyImportedPharmacy, pharmacyToAdd.Name, pharmacyToAdd.Medicines.Count));
                }
                catch (Exception ex)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
            }
            return sb.ToString();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
