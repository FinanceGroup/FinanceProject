namespace Finance.Framework.OAuth
{
    public interface IAuthorize : IDependency
    {
        Response Authorize(string userName);
    }
}
