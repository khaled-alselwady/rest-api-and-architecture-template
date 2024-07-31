using StudyCenterDataAccess.DTOs.UserDTOs;
using System.Data;

namespace StudyCenterDataAccess
{
    public static class Mappings
    {
        public static UserDTO MapToUserDto(IDataRecord record)
        {
            return new UserDTO
            (
                record.GetInt32(record.GetOrdinal("UserID")),
                record.GetInt32(record.GetOrdinal("PersonID")),
                record.GetString(record.GetOrdinal("Username")),
                record.GetString(record.GetOrdinal("Password")),
                record.GetInt32(record.GetOrdinal("Permissions")),
                record.GetBoolean(record.GetOrdinal("IsActive"))
            );
        }

        public static UserViewDTO MapToUserViewDto(IDataRecord record)
        {
            return new UserViewDTO
            (
                record.GetInt32(record.GetOrdinal("UserID")),
                record.GetString(record.GetOrdinal("FullName")),
                record.GetString(record.GetOrdinal("Username")),
                record.GetString(record.GetOrdinal("Gender")),
                record.GetBoolean(record.GetOrdinal("IsActive"))
            );
        }
    }
}
