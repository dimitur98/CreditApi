using Credit.Api.Db.Models;

namespace Credit.Api.Db.Repositories
{
    public interface ICreditRepository
    {
        Task<List<Models.Credit>> AllAsync();
        Task<List<CreditStatusReport>> GetStatusReportAsync(IEnumerable<uint> statusIds);
        Task LoadInvoicesAsync(IEnumerable<Models.Credit> credits);
    }
}