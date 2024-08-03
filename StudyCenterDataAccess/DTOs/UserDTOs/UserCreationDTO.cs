namespace StudyCenterDataAccess.DTOs.UserDTOs
{
    public record UserCreationDto(int? PersonID, string Username, string Password, int Permissions, bool IsActive);
}
