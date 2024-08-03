using StudyCenterDataAccess;
using StudyCenterDataAccess.DTOs.UserDTOs;
using static StudyCenterBusiness.clsUser;


namespace StudyCenterBusiness.UserFinders
{
    public class UsernamePasswordFinder : IUserFinder
    {
        public clsUser? FindUser(object? data)
        {
            if (data is (string username, string password))
            {
                UserDto? UserDTO = clsUserData.GetUserInfoByUsernameAndPassword(username, password);

                return (UserDTO != null) ? (new clsUser(UserDTO, enMode.Update)) : null;
            }

            return null;
        }
    }
}
