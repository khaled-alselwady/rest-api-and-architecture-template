using static StudyCenterBusiness.clsUser;

namespace StudyCenterBusiness.UserExistenceVerifiers
{
    public static class ExistenceVerifierFactory
    {
        public static IUserExistenceVerifier? GetExistenceVerifier(clsUser.enFindBy findBy)
        {
            switch (findBy)
            {
                case enFindBy.UserID:
                    return new ExistenceVerifierByUserId();
                case enFindBy.PersonID:
                    return new ExistenceVerifierByPersonId();
                case enFindBy.Username:
                    return new ExistenceVerifierByUsername();
                case enFindBy.UsernameAndPassword:
                    return new ExistenceVerifierByUsernameAndPassword();
                default:
                    return default;
            }
        }
    }
}
