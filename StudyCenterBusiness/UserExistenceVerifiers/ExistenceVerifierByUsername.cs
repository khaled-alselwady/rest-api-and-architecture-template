using StudyCenterDataAccess;

namespace StudyCenterBusiness.UserExistenceVerifiers
{
    public class ExistenceVerifierByUsername : IUserExistenceVerifier
    {
        public bool Exists(object? data)
        {
            if (data is string username)
            {
                return clsUserData.ExistsByUsername(username);
            }

            return false;
        }
    }
}
