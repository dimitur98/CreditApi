using Credit.Api.Db.Models;

namespace Credit.Api.Db.Repositories
{
    public interface IInvoiceRepository
    {
        Task<IEnumerable<Invoice>> GetByCreditIdsAsync(IEnumerable<uint> creditIds);
    }
}
