using StudyCenterDataAccess;
using StudyCenterDataAccess.DTOs.PersonDTOs;
using StudyCenterDataAccess.DTOs.UserDTOs;

namespace StudyCenterSharedDTOs.UserDTOs
{
    public record UserDetailsDto : UserDto
    {
        public PersonDto? PersonInfo { get; init; }
        public UserDetailsDto(int? userID, int? personID, string username, string password, int permissions, bool isActive)
            : base(userID, personID, username, password, permissions, isActive)
        {
            UserID = userID;
            PersonID = personID;
            Username = username;
            Password = password;
            Permissions = permissions;
            IsActive = isActive;
            PersonInfo = clsPersonData.GetInfoById(personID);
        }
    }
}
