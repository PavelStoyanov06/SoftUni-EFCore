namespace Cadastre.Data.Models
{
    public class PropertyCitizen
    {
        public int PropertyId { get; set; }

        public Property Property { get; set; }

        public int CitizenId { get; set; }

        public Citizen Citizen { get; set; }
    }
}

    //• PropertyId – integer, Primary Key, foreign key (required)
    //• Property – Property
    //• CitizenId – integer, Primary Key, foreign key (required)
    //• Citizen – Citizen