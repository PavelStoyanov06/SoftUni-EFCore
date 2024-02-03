using Medicines.Data.Models.Enums;
using Medicines.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicines.DataProcessor.ImportDtos
{
    public class ImportPatientDto
    {
        [Required]
        [MaxLength(100)]
        [MinLength(5)]
        public string FullName { get; set; }

        [Required]
        [Range(0, 2)]
        public AgeGroup AgeGroup { get; set; }

        [Required]
        [Range(0, 1)]
        public Gender Gender { get; set; }

        public int[] Medicines { get; set; }
    }
}

//• FullName – text with length [5, 100] (required)
//• AgeGroup – AgeGroup enum (Child = 0, Adult, Senior)(required)
//• Gender – Gender enum (Male = 0, Female)(required)
