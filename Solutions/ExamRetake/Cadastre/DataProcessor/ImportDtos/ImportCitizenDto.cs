using Cadastre.Data.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.DataProcessor.ImportDtos
{
    public class ImportCitizenDto
    {
        [Required]
        [MaxLength(30)]
        [MinLength(2)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(30)]
        [MinLength(2)]
        public string LastName { get; set; }

        [Required]
        public string BirthDate { get; set; }

        [Required]
        [RegularExpression(@"^(Unmarried|Married|Divorced|Widowed)$")]
        public string MaritalStatus { get; set; }

        public int[] Properties { get; set; }
    }
}

//• FirstName – text with length [2, 30] (required)
//• LastName – text with length [2, 30] (required)
//• BirthDate – DateTime (required)
//• MaritalStatus - MaritalStatus enum (Unmarried = 0, Married, Divorced, Widowed)(required)
