using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ImportDtos
{
    [XmlType("Property")]
    public class ImportPropertyDto
    {
        [Required]
        [MaxLength(20)]
        [MinLength(16)]
        public string PropertyIdentifier { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Area { get; set; }

        [MaxLength(500)]
        [MinLength(5)]
        public string? Details { get; set; }

        [Required]
        [MaxLength(200)]
        [MinLength(5)]
        public string Address { get; set; }

        [Required]
        public string DateOfAcquisition { get; set; }
    }
}

//• PropertyIdentifier – text with length [16, 20] (required)
//• Area – int not negative (required)
//• Details - text with length [5, 500] (not required)
//• Address – text with length [5, 200] (required)
//• DateOfAcquisition – DateTime (required)