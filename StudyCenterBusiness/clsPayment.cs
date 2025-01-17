﻿using StudyCenterDataAccess;
using System;
using System.Data;

namespace StudyCenterBusiness
{
    public class clsPayment
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? PaymentID { get; set; }
        public int? StudentGroupID { get; set; }
        public int? SubjectGradeLevelID { get; set; }
        public decimal PaymentAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public int? CreatedByUserID { get; set; }

        public clsStudentGroup StudentGroupInfo { get; private set; }
        public clsSubjectGradeLevel SubjectGradeLevelInfo { get; private set; }
        public clsUser CreatedByUserInfo { get; private set; }

        public clsPayment()
        {
            PaymentID = null;
            StudentGroupID = null;
            SubjectGradeLevelID = null;
            PaymentAmount = 0M;
            PaymentDate = DateTime.Now;
            CreatedByUserID = null;

            Mode = enMode.AddNew;
        }

        private clsPayment(int? paymentID, int? studentGroupID, int? subjectGradeLevelID,
            decimal paymentAmount, DateTime paymentDate, int? createdByUserID)
        {
            PaymentID = paymentID;
            StudentGroupID = studentGroupID;
            SubjectGradeLevelID = subjectGradeLevelID;
            PaymentAmount = paymentAmount;
            PaymentDate = paymentDate;
            CreatedByUserID = createdByUserID;

            StudentGroupInfo = clsStudentGroup.Find(studentGroupID);
            SubjectGradeLevelInfo = clsSubjectGradeLevel.Find(subjectGradeLevelID);
            CreatedByUserInfo = clsUser.FindBy(createdByUserID, clsUser.enFindBy.UserID);

            Mode = enMode.Update;
        }

        private bool _Validate()
        {
            if (Mode == enMode.Update && !PaymentID.HasValue)
            {
                return false;
            }

            if (!StudentGroupID.HasValue || !SubjectGradeLevelID.HasValue || !CreatedByUserID.HasValue)
            {
                return false;
            }

            if (PaymentAmount < 0)
            {
                return false;
            }

            if (Mode == enMode.AddNew && PaymentDate.Date < DateTime.Now.Date)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates the current instance of <see cref="clsPayment"/> using the <see cref="ValidationHelper"/>.
        /// </summary>
        /// <returns>
        /// Returns true if the current instance passes all validation checks; otherwise, false.
        /// </returns>
        private bool _ValidateUsingHelperClass()
        {
            return ValidationHelper.Validate
            (
            this,

            // ID Check: Ensure PaymentID is valid if in Update mode
            idCheck: payment => (payment.Mode != enMode.Update) || ValidationHelper.HasValue(payment.PaymentID),

            // Value Check: Ensure required properties are not null or empty
            valueCheck: payment => ValidationHelper.HasValue(payment.StudentGroupID) &&
                                   ValidationHelper.HasValue(payment.SubjectGradeLevelID) &&
                                   ValidationHelper.HasValue(payment.CreatedByUserID) &&
                                   payment.PaymentAmount >= 0,

            // Date Check: Ensure PaymentDate is not in the future if in AddNew mode
            dateCheck: payment => (payment.Mode != enMode.AddNew) ||
                                  ValidationHelper.DateIsNotValid(payment.PaymentDate, DateTime.Now)
            );
        }

        private bool _Add()
        {
            PaymentID = clsPaymentData.Add(StudentGroupID.Value, SubjectGradeLevelID.Value,
                PaymentAmount, CreatedByUserID.Value);

            return (PaymentID.HasValue);
        }

        private bool _Update()
        {
            return clsPaymentData.Update(PaymentID.Value, StudentGroupID.Value, SubjectGradeLevelID.Value,
                PaymentAmount, CreatedByUserID.Value);
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

        public static clsPayment Find(int? paymentID)
        {
            int? studentGroupID = null;
            int? subjectGradeLevelID = null;
            decimal paymentAmount = 0M;
            DateTime paymentDate = DateTime.Now;
            int? createdByUserID = null;

            bool isFound = clsPaymentData.GetInfoByID(paymentID, ref studentGroupID,
                ref subjectGradeLevelID, ref paymentAmount, ref paymentDate, ref createdByUserID);

            return (isFound) ? (new clsPayment(paymentID, studentGroupID,
                                subjectGradeLevelID, paymentAmount, paymentDate, createdByUserID))
                             : null;
        }

        public static bool Delete(int? paymentID)
        => clsPaymentData.Delete(paymentID);

        public static bool Exists(int? paymentID)
        => clsPaymentData.Exists(paymentID);

        public static DataTable All()
        => clsPaymentData.All();

        public static DataTable AllInPages(short PageNumber, int RowsPerPage)
            => clsPaymentData.AllInPages(PageNumber, RowsPerPage);

        public static int Count()
            => clsPaymentData.Count();
    }
}