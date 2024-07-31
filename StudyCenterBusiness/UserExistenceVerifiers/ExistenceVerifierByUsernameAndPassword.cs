using StudyCenterDataAccess;

namespace StudyCenterBusiness.UserExistenceVerifiers
{
    public class ExistenceVerifierByUsernameAndPassword : IUserExistenceVerifier
    {
        public bool Exists(object? data)
        {
            if (data is (string username, string password))
            {
                return clsUserData.ExistsByUsernameAndPassword(username, password);
            }

            return false;
        }
    }
}
