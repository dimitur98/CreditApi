using Dapper.Base;

namespace Credit.Api.Db.Repositories
{
    public abstract class BaseReposiotry
    {
        protected readonly IDapperBase _dapper;

        protected BaseReposiotry(IDapperBase dapper)
        {
            _dapper = dapper;
        }
    }
}