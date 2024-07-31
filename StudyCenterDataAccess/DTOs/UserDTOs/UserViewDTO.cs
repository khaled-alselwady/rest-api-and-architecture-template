namespace StudyCenterDataAccess.DTOs.UserDTOs
{
    public record UserViewDTO(int? UserID, string FullName, string Username, string Gender, bool IsActive);
}
