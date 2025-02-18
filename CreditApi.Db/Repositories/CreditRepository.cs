using Credit.Api.Db.Models;
using Dapper.Base;

namespace Credit.Api.Db.Repositories
{
    public class CreditRepository : BaseReposiotry, ICreditRepository
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public CreditRepository(IDapperBase dapper, IInvoiceRepository invoiceRepository) : base(dapper) 
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<List<Models.Credit>> AllAsync()
        {
            var sql = "SELECT * FROM Credit";

            return (await _dapper.QueryAsync<Models.Credit>(sql)).ToList();
        }

        public async Task<List<CreditStatusReport>> GetStatusReportAsync(IEnumerable<uint> statusIds)
        {
            var sql = @"WITH StatusTotalAmount AS (
                SELECT StatusId, SUM(amount) as TotalAmount
                FROM Credit
                WHERE StatusId IN @statusIds
                GROUP BY StatusId)

                SELECT s.Name AS Status, cs.TotalAmount, ROUND(cs.TotalAmount * 100.0 / SUM(cs.TotalAmount) OVER(),2) AS Percentage
                FROM StatusTotalAmount cs
                INNER JOIN CreditStatus s ON s.Id = cs.StatusId";

            return (await _dapper.QueryAsync<CreditStatusReport>(sql, param: new { statusIds })).ToList();
        }

        public async Task LoadInvoicesAsync(IEnumerable<Models.Credit> credits) 
        {
            var invoices = await _invoiceRepository.GetByCreditIdsAsync(credits.Select(c => c.Id));

            foreach (var credit in credits)
            {
                credit.Invoices = invoices.Where(i => i.CreditId == credit.Id);
            }
        }
    }
}