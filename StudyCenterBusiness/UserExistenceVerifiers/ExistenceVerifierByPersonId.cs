using StudyCenterDataAccess;

namespace StudyCenterBusiness.UserExistenceVerifiers
{
    public class ExistenceVerifierByPersonId : IUserExistenceVerifier
    {
        public bool Exists(object? data)
        {
            if (data is int personId)
            {
                return clsUserData.ExistsByPersonID(personId);
            }

            return false;
        }
    }
}
