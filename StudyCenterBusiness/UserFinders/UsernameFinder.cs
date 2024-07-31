using StudyCenterDataAccess;
using StudyCenterDataAccess.DTOs.UserDTOs;
using static StudyCenterBusiness.clsUser;


namespace StudyCenterBusiness.UserFinders
{
    public class UsernameFinder : IUserFinder
    {
        public clsUser? FindUser(object? data)
        {
            if (data is string username)
            {
                UserDTO? UserDTO = clsUserData.GetUserInfoByUsername(username);

                return (UserDTO != null) ? (new clsUser(UserDTO, enMode.Update)) : null;
            }

            return null;
        }
    }
}
