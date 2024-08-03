using StudyCenterDataAccess.DTOs.PersonDTOs;

namespace StudyCenterDataAccess
{
    public class clsPersonData
    {
        public static PersonDto? GetInfoById(int? personId)
            => clsDataAccessHelper.GetBy("SP_GetPersonInfoByID", "PersonID", personId, Mappings.MapToPersonDto);

        public static int? Add(PersonCreationDto personDto)
            => clsDataAccessHelper.Add("SP_AddNewPerson", "NewPersonID", personDto);

        public static bool Update(PersonDto personDto)
            => clsDataAccessHelper.Update("SP_UpdatePerson", personDto);

        public static bool Delete(int? personID)
            => clsDataAccessHelper.Delete("SP_DeletePerson", "PersonID", personID);

        public static bool Exists(int? personID)
            => clsDataAccessHelper.Exists("SP_DoesPersonExist", "PersonID", personID);

        public static List<PersonDto> All()
            => clsDataAccessHelper.All("SP_GetAllPeople", Mappings.MapToPersonDto);
    }
}