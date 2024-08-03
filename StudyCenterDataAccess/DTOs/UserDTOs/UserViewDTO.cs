namespace StudyCenterDataAccess.DTOs.UserDTOs
{
    public record UserViewDto(int? UserID, string FullName, string Username, string Gender, bool IsActive);
}
