using Invoices.Data.Models;

namespace Invoices.Data.Models
{
    public class ProductClient
    {
        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int ClientId { get; set; }

        public Client Client { get; set; }
    }
}

    //• ProductId – integer, Primary Key, foreign key (required)
    //• Product – Product
    //• ClientId – integer, Primary Key, foreign key (required)
    //• Client – Client