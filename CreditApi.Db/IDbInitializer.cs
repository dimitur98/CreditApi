namespace Credit.Api.Db
{
    public interface IDbInitializer
    {
        Task InitializeAsync();
    }
}
