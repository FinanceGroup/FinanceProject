using Finance.DAL.Records;
using Finance.Framework;

namespace Finance.DAL.DAOs
{
    public interface IUserDAO : IDependency
    {
        UserRecord GetRecord(string userName, string password);
    }
}
