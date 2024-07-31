namespace StudyCenterBusiness.UserFinders
{
    public interface IUserFinder
    {
        clsUser? FindUser(object? data);
    }
}
