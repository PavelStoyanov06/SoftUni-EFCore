using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ImportDtos
{
    [XmlType("Pharmacy")]
    public class ImportPharmacyDto
    {
        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^\(\d{3}\)\s\d{3}-\d{4}$")]
        public string PhoneNumber { get; set; }

        [Required]
        [XmlAttribute("non-stop")]
        [RegularExpression(@"^(true|false)$")]
        public string IsNonStop { get; set; }

        [XmlArray("Medicines")]
        public ImportMedicineDto[] Medicines { get; set; }
    }
}
//• Name – text with length [2, 50] (required)
//• PhoneNumber – text with length 14. (required)
//    ◦ All phone numbers must have the following structure: three digits enclosed in parentheses, followed by a space, three more digits, a hyphen, and four final digits: 
//        ▪ Example-> (123) 456 - 7890
//• IsNonStop – bool  (required)
//• Medicines - collection of type Medicine
