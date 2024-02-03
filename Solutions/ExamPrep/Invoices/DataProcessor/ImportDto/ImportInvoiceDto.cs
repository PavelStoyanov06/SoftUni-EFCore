using Invoices.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.DataProcessor.ImportDto
{
    public class ImportInvoiceDto
    {
        [Required]
        [Range(1000000000, 1500000000)]
        public int Number { get; set; }

        [Required]
        public DateTime IssueDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        [EnumDataType(typeof(CurrencyType))]
        public CurrencyType CurrencyType { get; set; }

        [Required]
        public int ClientId { get; set; }
    }
}

//• Number – integer in range  [1,000,000,000…1,500,000,000] (required)
//• IssueDate – DateTime (required)
//• DueDate – DateTime (required)
//• Amount – decimal (required)
//• CurrencyType – enumeration of type CurrencyType, with possible values (BGN, EUR, USD) (required)
//• ClientId – integer, foreign key (required)
