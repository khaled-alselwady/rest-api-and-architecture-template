using StudyCenterBusiness.UserExistenceVerifiers;
using StudyCenterBusiness.UserFinders;
using StudyCenterDataAccess;
using StudyCenterDataAccess.DTOs.UserDTOs;

namespace StudyCenterBusiness
{
    public class clsUser
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public UserDTO UserDto => new UserDTO(this.UserID, this.PersonID, this.Username, this.Password, this.Permissions, this.IsActive);
        public UserViewDTO UserViewDto => new UserViewDTO(this.UserID, this.PersonInfo.FullName, this.Username, this.PersonInfo.GenderName, this.IsActive);

        public enum enFindBy
        {
            UserID,
            PersonID,
            Username,
            UsernameAndPassword
        };

        public enum enPermissions
        {
            All = -1,
            AddUser = 1,
            UpdateUser = 2,
            DeleteUser = 4,
            ListUsers = 8
        }

        public int? UserID { get; set; }

        private int? _oldPersonID = null;
        private int? _personID = null;
        public int? PersonID
        {
            get => _personID;

            set
            {
                if (!_oldPersonID.HasValue)
                {
                    _oldPersonID = _personID;
                }

                _personID = value;
            }
        }

        private string _oldUsername = string.Empty;
        private string _userName = string.Empty;
        public string Username
        {
            get => _userName;

            set
            {
                // If the old username is not set (indicating either a new user or the username is being set for the first time),
                // initialize it with the current username value to track changes.
                if (string.IsNullOrWhiteSpace(_oldUsername))
                {
                    _oldUsername = _userName;
                }

                _userName = value;
            }
        }

        public string Password { get; set; }
        public int Permissions { get; set; }
        public bool IsActive { get; set; }

        public clsPerson PersonInfo { get; private set; }

        public clsUser()
        {
            UserID = null;
            PersonID = null;
            Username = string.Empty;
            Password = string.Empty;
            Permissions = -1;
            IsActive = true;

            Mode = enMode.AddNew;
        }

        public clsUser(UserDTO UserDTO, enMode mode = enMode.AddNew)
        {
            UserID = UserDTO.UserID;
            PersonID = UserDTO.PersonID;
            Username = UserDTO.Username;
            Password = UserDTO.Password;
            Permissions = UserDTO.Permissions;
            IsActive = UserDTO.IsActive;

            PersonInfo = clsPerson.Find(UserDTO.PersonID);

            Mode = mode;
        }

        private bool _Validate()
        {
            if (Mode == enMode.Update && !UserID.HasValue)
            {
                return false;
            }

            if (!PersonID.HasValue)
            {
                return false;
            }

            if ((Mode == enMode.AddNew) || _oldPersonID != _personID)
            {
                if (Exists(_personID, enFindBy.PersonID))
                {
                    return false;
                }
            }

            if (string.IsNullOrWhiteSpace(_userName))
            {
                return false;
            }

            // If the old username is different from the new username:
            // - In AddNew Mode: This indicates the new username, requiring validation.
            // - In Update Mode: This indicates that the username has been changed, so we need to check if it already exists in the database.
            // If the new username already exists in the database, return false to indicate validation failure.
            if ((Mode == enMode.AddNew) || (_oldUsername.Trim().ToLower() != _userName.Trim().ToLower()))
            {
                if (Exists(_userName, enFindBy.Username))
                {
                    return false;
                }
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates the current instance of <see cref="clsUser"/> using the <see cref="clsValidationHelper"/>.
        /// </summary>
        /// <returns>
        /// Returns true if the current instance passes all validation checks; otherwise, false.
        /// </returns>
        private bool _ValidateUsingHelperClass()
        {
            return clsValidationHelper.Validate
            (
            this,

            // ID Check: Ensure UserID is valid if in Update mode
            idCheck: user => (Mode != enMode.Update || clsValidationHelper.HasValue(user.UserID)),

            // Value Check: Ensure PersonID is provided
            valueCheck: user => clsValidationHelper.HasValue(user.PersonID),

            // Additional Checks: Check various conditions and provide corresponding error messages
            additionalChecks: new (Func<clsUser, bool>, string)[]
            {
                // Check if PersonID already exists, considering mode and previous value
                (user => (Mode != enMode.AddNew && _oldPersonID == user.PersonID) ||
                         !clsValidationHelper.ExistsInDatabase(() => Exists(user.PersonID, enFindBy.PersonID)),
                         "Person already exists."),

                // Check if Username is not empty
                (user => clsValidationHelper.IsNotEmpty(user.Username), "Username is empty."),

                // Check if Username already exists, considering mode and previous value
                (user => (Mode != enMode.AddNew && _oldUsername.Trim().ToLower() == user.Username.Trim().ToLower()) ||
                         !clsValidationHelper.ExistsInDatabase(() => Exists(user.Username, enFindBy.Username)),
                         "Username already exists."),

                // Check if Password is not empty
                (user => clsValidationHelper.IsNotEmpty(user.Password), "Password is empty.")
            }
            );
        }

        private bool _Add()
        {
            UserID = clsUserData.Add(new UserCreationDTO(this.PersonID, this.Username, this.Password, this.Permissions, this.IsActive));

            return (UserID.HasValue);
        }

        private bool _Update()
        {
            return clsUserData.Update(UserDto);
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

        public static clsUser? FindBy<T>(T data, enFindBy findBy)
        {
            var finder = UserFinderFactory.GetFinder(findBy);
            return finder.FindUser(data);
        }

        public static clsUser? FindBy(string username, string password)
        {
            var finder = UserFinderFactory.GetFinder(enFindBy.UsernameAndPassword);
            return finder.FindUser((username, password));
        }

        public static bool Delete(int? userID)
            => clsUserData.Delete(userID);

        public static bool Exists(object data, enFindBy itemToFindBy)
        {
            var verifier = ExistenceVerifierFactory.GetExistenceVerifier(itemToFindBy);

            return (verifier != null) && verifier.Exists(data);
        }

        public static bool Exists(string username, string password)
        {
            var verifier = ExistenceVerifierFactory.GetExistenceVerifier(enFindBy.UsernameAndPassword);
            return (verifier != null) && verifier.Exists((username, password));
        }

        public static List<UserViewDTO> All()
            => clsUserData.AllUsers();

        public static int Count()
            => clsUserData.Count();

        public bool ChangePassword(string NewPassword)
            => ChangePassword(UserID, NewPassword);

        public static bool ChangePassword(int? UserID, string NewPassword)
            => clsUserData.ChangePassword(UserID, NewPassword);

        public static List<string> GetPermissionsText(int permissionUser)
        {
            if (permissionUser == -1)
            {
                return new List<string>() { "Admin" };
            }

            List<string> permissions = new List<string>();

            if (((int)enPermissions.AddUser & permissionUser) == (int)enPermissions.AddUser)
            {
                permissions.Add("Add User");
            }

            if (((int)enPermissions.UpdateUser & permissionUser) == (int)enPermissions.UpdateUser)
            {
                permissions.Add("Update User");
            }

            if (((int)enPermissions.DeleteUser & permissionUser) == (int)enPermissions.DeleteUser)
            {
                permissions.Add("Delete User");
            }

            if (((int)enPermissions.ListUsers & permissionUser) == (int)enPermissions.ListUsers)
            {
                permissions.Add("List Users");
            }

            return permissions;
        }

        public List<string> PermissionsText()
        {
            return GetPermissionsText(this.Permissions);
        }
    }
}