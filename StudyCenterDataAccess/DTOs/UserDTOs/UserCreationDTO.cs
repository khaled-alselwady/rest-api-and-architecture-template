namespace StudyCenterDataAccess.DTOs.UserDTOs
{
    public record UserCreationDTO(int? PersonID, string Username, string Password, int Permissions, bool IsActive);
}
