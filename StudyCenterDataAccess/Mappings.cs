using StudyCenterDataAccess.DTOs.PersonDTOs;
using StudyCenterDataAccess.DTOs.UserDTOs;
using StudyCenterSharedDTOs.UserDTOs;
using System.Data;

namespace StudyCenterDataAccess
{
    public static class Mappings
    {
        public static UserDto MapToUserDto(IDataRecord record)
        {
            return new UserDto
            (
                record.GetInt32(record.GetOrdinal("UserID")),
                record.GetInt32(record.GetOrdinal("PersonID")),
                record.GetString(record.GetOrdinal("Username")),
                record.GetString(record.GetOrdinal("Password")),
                record.GetInt32(record.GetOrdinal("Permissions")),
                record.GetBoolean(record.GetOrdinal("IsActive"))
            );
        }

        public static UserDetailsDto MapToUserDetailsDto(IDataRecord record)
        {
            return new UserDetailsDto
            (
                record.GetInt32(record.GetOrdinal("UserID")),
                record.GetInt32(record.GetOrdinal("PersonID")),
                record.GetString(record.GetOrdinal("Username")),
                record.GetString(record.GetOrdinal("Password")),
                record.GetInt32(record.GetOrdinal("Permissions")),
                record.GetBoolean(record.GetOrdinal("IsActive"))
            );
        }

        public static UserViewDto MapToUserViewDto(IDataRecord record)
        {
            return new UserViewDto
            (
                record.GetInt32(record.GetOrdinal("UserID")),
                record.GetString(record.GetOrdinal("FullName")),
                record.GetString(record.GetOrdinal("Username")),
                record.GetString(record.GetOrdinal("Gender")),
                record.GetBoolean(record.GetOrdinal("IsActive"))
            );
        }

        public static PersonDto MapToPersonDto(IDataRecord record)
        {
            return new PersonDto
            (
                record.GetInt32(record.GetOrdinal("PersonID")),
                record.GetString(record.GetOrdinal("FirstName")),
                record.GetString(record.GetOrdinal("SecondName")),
                record.GetValue(record.GetOrdinal("ThirdName")) as string ?? null,
                record.GetString(record.GetOrdinal("LastName")),
                record.GetByte(record.GetOrdinal("Gender")),
                record.GetDateTime(record.GetOrdinal("DateOfBirth")),
                record.GetString(record.GetOrdinal("PhoneNumber")),
                record.GetValue(record.GetOrdinal("Email")) as string ?? null,
                record.GetValue(record.GetOrdinal("Address")) as string ?? null
            );
        }
    }
}
