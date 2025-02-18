namespace Credit.Api.Db.Models
{
    public class Invoice : BaseModel
    {
        public string InvoiceNumber { get; set; }

        public decimal Amount { get; set; }

        public int CreditId { get; set; }
    }
}