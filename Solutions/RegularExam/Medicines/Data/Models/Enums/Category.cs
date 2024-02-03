using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Medicines.Data.Models.Enums
{
    public enum Category
    {
        [XmlEnum("1")]
        Analgesic = 0,
        [XmlEnum("2")]
        Antibiotic,
        [XmlEnum("3")]
        Antiseptic,
        [XmlEnum("4")]
        Sedative,
        [XmlEnum("5")]
        Vaccine
    }
}
