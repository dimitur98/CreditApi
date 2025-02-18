namespace Credit.Api.Db.Models
{
    public class Credit : BaseModel
    {
        public string CreditNumber { get; set; }

        public string CustomerName { get; set; }

        public decimal Amount { get; set; }

        public DateTime DateRequested { get; set; }

        public uint StatusId { get; set; }
        public  CreditStatus Status { get; set; }

        public IEnumerable<Invoice> Invoices { get; set; }
    }
}