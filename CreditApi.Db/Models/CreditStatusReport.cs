namespace Credit.Api.Db.Models
{
    public class CreditStatusReport
    {
        public string Status { get; set; }

        public decimal TotalAmount { get; set; }

        public double Percentage { get; set; }
    }
}