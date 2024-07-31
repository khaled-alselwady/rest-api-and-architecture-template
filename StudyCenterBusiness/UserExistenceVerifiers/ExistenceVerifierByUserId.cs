using StudyCenterDataAccess;

namespace StudyCenterBusiness.UserExistenceVerifiers
{
    public class ExistenceVerifierByUserId : IUserExistenceVerifier
    {
        public bool Exists(object? data)
        {
            if (data is int userId)
            {
                return clsUserData.ExistsByUserID(userId);
            }

            return false;
        }
    }
}
