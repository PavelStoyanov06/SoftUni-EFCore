using Cadastre.Data.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ImportDtos
{
    [XmlType("District")]
    public class ImportDistrictDto
    {
        [Required]
        [MaxLength(80)]
        [MinLength(2)]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]{2}-\d{5}$")]
        public string PostalCode { get; set; }

        [Required]
        //[Range(0, 3)]
        //[EnumDataType(typeof(Region))]
        [RegularExpression(@"^(South|North)(East|West)$")]
        [XmlAttribute("Region")]
        public string Region { get; set; }

        [XmlArray("Properties")]
        public ImportPropertyDto[] Properties { get; set; }
    }
}

//• Name – text with length [2, 80] (required)
//• PostalCode – text with length 8. All postal codes must have the following structure:starting with two capital letters, followed by e dash '-', followed by five digits. Example: SF - 10000(required)
//• Region – Region enum (SouthEast = 0, SouthWest, NorthEast, NorthWest)(required)
