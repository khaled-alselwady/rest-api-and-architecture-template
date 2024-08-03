using Microsoft.Data.SqlClient;
using StudyCenterDataAccess.DTOs.UserDTOs;
using StudyCenterSharedDTOs.UserDTOs;
using System.Data;


namespace StudyCenterDataAccess
{
    public class clsUserData
    {
        public static UserDetailsDto? GetUserInfoByUserID(int? userID)
            => clsDataAccessHelper.GetBy("SP_GetUserInfoByID", "UserID", userID, Mappings.MapToUserDetailsDto);

        public static UserDetailsDto? GetUserInfoByPersonID(int? personID)
            => clsDataAccessHelper.GetBy("SP_GetUserInfoByPersonID", "PersonID", personID, Mappings.MapToUserDetailsDto);

        public static UserDetailsDto? GetUserInfoByUsername(string username)
            => clsDataAccessHelper.GetBy("SP_GetUserInfoByUsername", "Username", username, Mappings.MapToUserDetailsDto);

        public static UserDetailsDto? GetUserInfoByUsernameAndPassword(string username, string password)
        {
            var parameters = new (string name, object? value)[]
            {
                (name: "Username", value: username),
                (name: "Password", value: password)
            };

            return clsDataAccessHelper.GetBy("SP_GetUserInfoByUsernameAndPassword", parameters, Mappings.MapToUserDetailsDto);
        }

        public static int? Add(UserCreationDto userDto)
           => clsDataAccessHelper.Add("SP_AddNewUser", "NewUserID", userDto);

        public static bool Update(UserDto userDto)
            => clsDataAccessHelper.Update("SP_UpdateUser", userDto);

        public static bool Delete(int? userID)
            => clsDataAccessHelper.Delete("SP_DeleteUser", "UserID", userID);

        public static bool ExistsByUserID(int? userID)
            => clsDataAccessHelper.Exists("SP_DoesUserExistByUserID", "UserID", userID);

        public static bool ExistsByPersonID(int? personID)
            => clsDataAccessHelper.Exists("SP_DoesUserExistByPersonID", "PersonID", personID);

        public static bool ExistsByUsername(string username)
            => clsDataAccessHelper.Exists("SP_DoesUserExistByUsername", "Username", username);

        public static bool ExistsByUsernameAndPassword(string username, string password)
            => clsDataAccessHelper.Exists("SP_DoesUserExistByUsernameAndPassword", "Username", username, "Password", password);

        public static List<UserViewDto> AllUsers()
            => clsDataAccessHelper.All("Sp_GetAllUsers", Mappings.MapToUserViewDto);

        public static int Count()
            => clsDataAccessHelper.Count("SP_GetAllUsersCount");

        public static bool ChangePassword(int? UserID, string NewPassword)
        {
            int RowAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SP_ChangePassword", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@UserID", (object)UserID ?? DBNull.Value);
                        command.Parameters.AddWithValue("@NewPassword", NewPassword);

                        RowAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                clsDataAccessHelper.HandleException(ex);
            }

            return (RowAffected > 0);
        }
    }
}