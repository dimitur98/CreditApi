using Credit.Api.Db;
using Credit.Api.Db.Models;
using Credit.Api.Db.Repositories;

namespace Credit.Api.Tests
{
    public class CreditRepositoryTests : IAsyncLifetime
    {
        private ICreditRepository _creditRepository;

        public async Task InitializeAsync()
        {
            var db = new CreditApiDb("Data Source=file:creditApiDbTests?mode=memory&cache=shared");
            var invoiceRepository = new InvoiceRepository(db);
            var dbInitializer = new DbInitializer(db);

            await dbInitializer.InitializeAsync();

            _creditRepository = new CreditRepository(db, invoiceRepository);
        }

        public Task DisposeAsync() => Task.CompletedTask;

        [Fact]
        public async Task GetStatusReportAsync_ShouldReturnCorrectReportForPaidAndAwaitingPayment()
        {
            var statusReport = new List<CreditStatusReport>
            {
                new CreditStatusReport { Status = "Paid", TotalAmount = 5000, Percentage = 29.41 },
                new CreditStatusReport { Status = "AwaitingPayment", TotalAmount = 12000, Percentage = 70.59 }
            };

            var result = await _creditRepository.GetStatusReportAsync([CreditStatusRepository.Paid, CreditStatusRepository.AwaitingPayment]);

            Assert.Equal(2, result.Count());
            Assert.Equivalent(result, statusReport);
        }

        [Fact]
        public async Task LoadInvoicesAsync_ShouldAssignInvoicesToCorrectCredits()
        {
            var credits = await _creditRepository.AllAsync();

            await _creditRepository.LoadInvoicesAsync(credits);

            Assert.NotEmpty(credits);

            var credit1 = credits.FirstOrDefault(c => c.CreditNumber == "CRD001");
            var credit2 = credits.FirstOrDefault(c => c.CreditNumber == "CRD002");

            Assert.NotNull(credit1);
            Assert.NotNull(credit2);

            Assert.Equal(2, credit1.Invoices.Count()); // CRD001 should have 2 invoices
            Assert.Single(credit2.Invoices); // CRD002 should have 1 invoice

            Assert.Contains(credit1.Invoices, i => i.InvoiceNumber == "INV001");
            Assert.Contains(credit1.Invoices, i => i.InvoiceNumber == "INV003");
            Assert.Contains(credit2.Invoices, i => i.InvoiceNumber == "INV004");
        }
    }
}