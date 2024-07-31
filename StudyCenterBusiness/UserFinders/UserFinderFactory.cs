using static StudyCenterBusiness.clsUser;


namespace StudyCenterBusiness.UserFinders
{
    public static class UserFinderFactory
    {
        public static IUserFinder? GetFinder(enFindBy findBy)
        {
            switch (findBy)
            {
                case enFindBy.UserID:
                    return new UserIdFinder();
                case enFindBy.PersonID:
                    return new PersonIdFinder();
                case enFindBy.Username:
                    return new UsernameFinder();
                case enFindBy.UsernameAndPassword:
                    return new UsernamePasswordFinder();
                default:
                    return default;
            }
        }
    }
}
