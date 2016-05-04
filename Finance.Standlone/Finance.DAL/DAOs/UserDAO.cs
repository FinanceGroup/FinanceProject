using Finance.DAL.Records;
using Finance.Framework.Data;

namespace Finance.DAL.DAOs
{
    public class UserDAO : IUserDAO
    {
        private readonly IRepository<UserRecord> _userRecordRepository = null;
        public UserDAO(IRepository<UserRecord> userRecordRepository)
        {
            _userRecordRepository = userRecordRepository;
        }

        public UserRecord GetRecord(string userName, string password)
        {
            var record = _userRecordRepository.Get(o => o.UserName == userName && o.Password == password);
            return record;
        }
    }
}
