namespace Finance.Framework.OAuth
{
    public interface IAuthenticate : IDependency
    {
        Response Authenticate(string userName, string password);
    }
}
