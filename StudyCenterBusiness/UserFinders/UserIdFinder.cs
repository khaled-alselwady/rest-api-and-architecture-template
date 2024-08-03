using StudyCenterDataAccess;
using StudyCenterDataAccess.DTOs.UserDTOs;
using static StudyCenterBusiness.clsUser;

namespace StudyCenterBusiness.UserFinders
{
    public class UserIdFinder : IUserFinder
    {
        public clsUser? FindUser(object? data)
        {
            if (data is int userId)
            {
                UserDto? UserDTO = clsUserData.GetUserInfoByUserID(userId);

                return (UserDTO != null) ? (new clsUser(UserDTO, enMode.Update)) : null;
            }

            return null;
        }
    }
}
