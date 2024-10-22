using StudyCenterDataAccess;
using System;
using System.Data;

namespace StudyCenterBusiness
{
    public class clsStudentGroup
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? StudentGroupID { get; set; }
        public int? StudentID { get; set; }
        public int? GroupID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedByUserID { get; set; }

        public clsStudent StudentInfo { get; private set; }
        public clsGroup GroupInfo { get; private set; }
        public clsUser CreatedByUserInfo { get; private set; }

        public clsStudentGroup()
        {
            StudentGroupID = null;
            StudentID = null;
            GroupID = null;
            StartDate = DateTime.Now;
            EndDate = null;
            IsActive = true;

            Mode = enMode.AddNew;
        }

        private clsStudentGroup(int? studentGroupID, int? studentID,
            int? groupID, DateTime startDate, DateTime? endDate, bool isActive, int? createdByUserID)
        {
            StudentGroupID = studentGroupID;
            StudentID = studentID;
            GroupID = groupID;
            StartDate = startDate;
            EndDate = endDate;
            IsActive = isActive;

            StudentInfo = clsStudent.FindByStudentID(studentID);
            GroupInfo = clsGroup.Find(groupID);
            CreatedByUserInfo = clsUser.FindBy(createdByUserID, clsUser.enFindBy.UserID);

            Mode = enMode.Update;
        }

        private bool _Validate()
        {
            if (Mode == enMode.Update && !StudentGroupID.HasValue)
            {
                return false;
            }

            if (!StudentID.HasValue || !GroupID.HasValue || !CreatedByUserID.HasValue)
            {
                return false;
            }

            if (Mode == enMode.AddNew && StartDate.Date < DateTime.Now.Date)
            {
                return false;
            }

            if (Mode == enMode.Update && EndDate.HasValue && StartDate.Date > EndDate.Value.Date)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates the current instance of <see cref="clsStudentGroup"/> using the <see cref="ValidationHelper"/>.
        /// </summary>
        /// <returns>
        /// Returns true if the current instance passes all validation checks; otherwise, false.
        /// </returns>
        private bool _ValidateUsingHelperClass()
        {
            return ValidationHelper.Validate
            (
            this,

            // ID Check: Ensure StudentGroupID is valid if in Update mode
            idCheck: studentGroup => (Mode != enMode.Update || ValidationHelper.HasValue(studentGroup.StudentGroupID)),

            // Value Check: Ensure required properties are not null
            valueCheck: studentGroup => ValidationHelper.HasValue(studentGroup.StudentID) &&
                            ValidationHelper.HasValue(studentGroup.GroupID) &&
                            ValidationHelper.HasValue(studentGroup.CreatedByUserID),

            // Date Check: Ensure dates are valid
            dateCheck: studentGroup => (Mode == enMode.AddNew && ValidationHelper.DateIsNotValid(studentGroup.StartDate, DateTime.Now)) ||
                                       (Mode == enMode.Update && (!studentGroup.EndDate.HasValue || ValidationHelper.DateIsNotValid(studentGroup.StartDate, studentGroup.EndDate.Value)))
            );
        }

        private bool _Add()
        {
            StudentGroupID = clsStudentGroupData.Add(StudentID.Value, GroupID.Value, CreatedByUserID.Value);

            return (StudentGroupID.HasValue);
        }

        private bool _Update()
        {
            return clsStudentGroupData.Update(StudentGroupID.Value, StudentID.Value, GroupID.Value, EndDate, IsActive);
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

        public static clsStudentGroup Find(int? studentGroupID)
        {
            int? studentID = null;
            int? groupID = null;
            DateTime startDate = DateTime.Now;
            DateTime? endDate = null;
            bool isActive = true;
            int? createdByUserID = null;

            bool isFound = clsStudentGroupData.GetInfoByID(studentGroupID, ref studentID,
                ref groupID, ref startDate, ref endDate, ref isActive, ref createdByUserID);

            return (isFound) ? (new clsStudentGroup(studentGroupID, studentID, groupID,
                startDate, endDate, isActive, createdByUserID)) : null;
        }

        public static bool Delete(int? studentGroupID)
            => clsStudentGroupData.Delete(studentGroupID);

        public static bool Delete(int? studentID, int? groupID)
            => clsStudentGroupData.Delete(studentID, groupID);

        public static bool Exists(int? studentGroupID)
            => clsStudentGroupData.Exists(studentGroupID);

        public static bool IsStudentAssignedToGroup(int? studentID, int? groupID)
            => clsStudentGroupData.IsStudentAssignedToGroup(studentID, groupID);

        public static DataTable All()
            => clsStudentGroupData.All();

        public static DataTable AllAvailableGroupsForStudent(int? studentID)
            => clsStudentGroupData.AllAvailableGroupsForStudent(studentID);
    }
}