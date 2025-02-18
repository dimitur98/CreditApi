using Credit.Api.Db.Repositories;

namespace Credit.Api.Endpoints
{
    public static class Credits
    {
        public static void RegisterCreditEnpoints(this IEndpointRouteBuilder routes)
        {
            var credits = routes.MapGroup("/api/Credits");

            credits.MapGet("/All", async (ICreditRepository creditRepository) =>
            {
                var credits = await creditRepository.AllAsync();

                await creditRepository.LoadInvoicesAsync(credits);

                return Results.Ok(credits);
            });

            credits.MapGet("/StatusReport", async (ICreditRepository creditRepository) =>
            {
                var statusReports = await creditRepository.GetStatusReportAsync([CreditStatusRepository.Paid, CreditStatusRepository.AwaitingPayment]);

                return Results.Ok(statusReports);
            });
        }
    }
}