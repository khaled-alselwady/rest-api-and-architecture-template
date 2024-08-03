using StudyCenterDataAccess;
using StudyCenterDataAccess.DTOs.UserDTOs;
using static StudyCenterBusiness.clsUser;

namespace StudyCenterBusiness.UserFinders
{
    public class PersonIdFinder : IUserFinder
    {
        public clsUser? FindUser(object? data)
        {
            if (data is int personId)
            {
                UserDto? UserDTO = clsUserData.GetUserInfoByPersonID(personId);

                return (UserDTO != null) ? (new clsUser(UserDTO, enMode.Update)) : null;
            }

            return null;
        }
    }
}
