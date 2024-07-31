namespace StudyCenterDataAccess.DTOs.UserDTOs
{
    //public class UserDTO
    //{
    //    public int? UserID { get; set; }
    //    public int? PersonID { get; set; }
    //    public string Username { get; set; }
    //    public string Password { get; set; }
    //    public int Permissions { get; set; }
    //    public bool IsActive { get; set; }

    //    public UserDTO(int? userID, int? personID, string username, string password, int permissions, bool isActive)
    //    {
    //        UserID = userID;
    //        PersonID = personID;
    //        Username = username;
    //        Password = password;
    //        Permissions = permissions;
    //        IsActive = isActive;
    //    }
    //}

    public record UserDTO(int? UserID, int? PersonID, string Username, string Password, int Permissions, bool IsActive);
}
