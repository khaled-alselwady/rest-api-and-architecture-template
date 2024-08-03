using StudyCenterDataAccess;
using StudyCenterDataAccess.DTOs.PersonDTOs;

namespace StudyCenterBusiness
{
    public class clsPerson
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public PersonDto ToPersonDto() => new PersonDto(this.PersonID, this.FirstName, this.SecondName, this.ThirdName, this.LastName, (byte)this.Gender, this.DateOfBirth, this.PhoneNumber, this.Email, this.Address);

        public enum enGender { Male = 0, Female = 1 };

        public int? PersonID { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string? ThirdName { get; set; }
        public string LastName { get; set; }
        public enGender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }

        public string FullName => string.Concat(FirstName, " ", SecondName, " ", ThirdName ?? "", " ", LastName);
        public string GenderName => Gender.ToString();

        public bool IsUser => clsUser.Exists(PersonID, clsUser.enFindBy.PersonID);
        public bool IsStudent => clsStudent.IsStudent(PersonID);
        public bool IsTeacher => clsTeacher.IsTeacher(PersonID);

        public clsPerson(PersonDto personDto, enMode mode = enMode.AddNew)
        {
            PersonID = personDto.PersonID;
            FirstName = personDto.FirstName;
            SecondName = personDto.SecondName;
            ThirdName = personDto.ThirdName;
            LastName = personDto.LastName;
            Gender = (enGender)personDto.Gender;
            DateOfBirth = personDto.DateOfBirth;
            PhoneNumber = personDto.PhoneNumber;
            Email = personDto.Email;
            Address = personDto.Address;

            Mode = mode;
        }


        /// <summary>
        /// Validates the current instance of <see cref="clsPerson"/> using the <see cref="ValidationHelper"/>.
        /// </summary>
        /// <returns>
        /// Returns true if the current instance passes all validation checks; otherwise, false.
        /// </returns>
        private bool _ValidateUsingHelperClass()
        {
            return ValidationHelper.Validate
            (
            this,

            // ID Check: Ensure PersonID is valid if in Update mode
            idCheck: person => person.Mode != enMode.Update || ValidationHelper.HasValue(person.PersonID),

            // Value Check: Ensure required properties are not null or empty
            valueCheck: person => !string.IsNullOrWhiteSpace(person.FirstName) &&
                                  !string.IsNullOrWhiteSpace(person.SecondName) &&
                                  !string.IsNullOrWhiteSpace(person.LastName) &&
                                  !string.IsNullOrWhiteSpace(person.PhoneNumber),

            // Date Check: Ensure DateOfBirth is not in the future
            dateCheck: person => ValidationHelper.DateIsNotValid(person.DateOfBirth, DateTime.Now),

            // Additional Checks: Ensure Gender value is 0 or 1 and provide corresponding error messages
            additionalChecks: new (Func<clsPerson, bool>, string)[]
            {
                (person => (person.Gender == 0 || (int)person.Gender == 1),
                            "Gender is wrong."),
            }
            );
        }

        private bool _Add()
        {
            PersonID = clsPersonData.Add(new PersonCreationDto(this.FirstName, this.SecondName,
                                              this.ThirdName, this.LastName, (byte)this.Gender,
                                              this.DateOfBirth, this.PhoneNumber, this.Email, this.Address)
                                        );

            return (PersonID.HasValue);
        }

        private bool _Update()
        {
            return clsPersonData.Update(this.ToPersonDto());
        }

        public bool Save()
        {
            if (!_ValidateUsingHelperClass())
            {
                return false;
            }

            switch (Mode)
            {
                case enMode.AddNew:
                    if (_Add())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _Update();
            }

            return false;
        }

        public bool TryToSave(out bool isValidationError)
        {
            if (!_ValidateUsingHelperClass())
            {
                isValidationError = true;
                return false;
            }

            switch (Mode)
            {
                case enMode.AddNew:
                    if (_Add())
                    {
                        Mode = enMode.Update;
                        isValidationError = false;
                        return true;
                    }
                    else
                    {
                        isValidationError = false;
                        return false;
                    }

                case enMode.Update:
                    isValidationError = false;
                    return _Update();
            }

            isValidationError = false;
            return false;
        }

        public static clsPerson? Find(int? personId)
        {
            PersonDto? personDto = clsPersonData.GetInfoById(personId);

            return (personDto != null) ? (new clsPerson(personDto, enMode.Update)) : null;
        }

        public static bool Delete(int? personID) => clsPersonData.Delete(personID);

        public static bool Exists(int? personID) => clsPersonData.Exists(personID);

        public static List<PersonDto> All() => clsPersonData.All();
    }
}