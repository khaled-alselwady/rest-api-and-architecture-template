namespace StudyCenterBusiness.UserExistenceVerifiers
{
    public interface IUserExistenceVerifier
    {
        bool Exists(object? data);
    }
}
