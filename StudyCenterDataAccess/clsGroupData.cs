using Microsoft.Data.SqlClient;
using System.Data;

namespace StudyCenterDataAccess
{
    public class clsGroupData
    {
        public static bool GetInfoByID(int? groupID, ref string groupName,
            ref int? classID, ref int? teacherID, ref int? subjectTeacherID,
            ref int? meetingTimeID, ref byte studentCount, ref int? createdByUserID,
            ref DateTime creationDate, ref DateTime? lastModifiedDate, ref bool isActive)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SP_GetGroupInfoByID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@GroupID", (object)groupID ?? DBNull.Value);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                groupName = (string)reader["GroupName"];
                                classID = (reader["ClassID"] != DBNull.Value) ? (int?)reader["ClassID"] : null;
                                teacherID = (reader["TeacherID"] != DBNull.Value) ? (int?)reader["TeacherID"] : null;
                                subjectTeacherID = (reader["SubjectTeacherID"] != DBNull.Value) ? (int?)reader["SubjectTeacherID"] : null;
                                meetingTimeID = (reader["MeetingTimeID"] != DBNull.Value) ? (int?)reader["MeetingTimeID"] : null;
                                studentCount = Convert.ToByte(reader["StudentCount"]);
                                createdByUserID = (reader["CreatedByUserID"] != DBNull.Value) ? (int?)reader["CreatedByUserID"] : null;
                                creationDate = (DateTime)reader["CreationDate"];
                                lastModifiedDate = (reader["LastModifiedDate"] != DBNull.Value) ? (DateTime?)reader["LastModifiedDate"] : null;
                                isActive = (bool)reader["IsActive"];
                            }
                            else
                            {
                                // The record was not found
                                isFound = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isFound = false;
                clsDataAccessHelper.HandleException(ex);
            }

            return isFound;
        }

        public static int? Add(int classID, int teacherID, int subjectTeacherID,
             int meetingTimeID, int createdByUserID)
        {
            // This function will return the new person id if succeeded and null if not
            int? groupID = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SP_AddNewGroup", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ClassID", classID);
                        command.Parameters.AddWithValue("@TeacherID", teacherID);
                        command.Parameters.AddWithValue("@SubjectTeacherID", subjectTeacherID);
                        command.Parameters.AddWithValue("@MeetingTimeID", meetingTimeID);
                        command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);

                        object result = command.ExecuteScalar();

                        groupID = (result != null) ? (int?)result : null;
                    }
                }
            }
            catch (Exception ex)
            {
                clsDataAccessHelper.HandleException(ex);
            }

            return groupID;
        }

        public static bool Update(int groupID, int classID,
            int teacherID, int subjectTeacherID, int meetingTimeID,
             bool isActive)
        {
            int rowAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SP_UpdateGroup", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@GroupID", groupID);
                        command.Parameters.AddWithValue("@ClassID", classID);
                        command.Parameters.AddWithValue("@TeacherID", teacherID);
                        command.Parameters.AddWithValue("@SubjectTeacherID", subjectTeacherID);
                        command.Parameters.AddWithValue("@MeetingTimeID", meetingTimeID);
                        command.Parameters.AddWithValue("@IsActive", isActive);

                        rowAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                clsDataAccessHelper.HandleException(ex);
            }

            return (rowAffected > 0);
        }

        public static bool Delete(int? groupID)
            => clsDataAccessHelper.Delete("SP_DeleteGroup", "GroupID", groupID);

        public static bool Exists(int? groupID)
            => clsDataAccessHelper.Exists("SP_DoesGroupExist", "GroupID", groupID);

        public static DataTable All()
            => clsDataAccessHelper.All("SP_GetAllGroups");

        public static DataTable AllInPages(short PageNumber, int RowsPerPage)
           => clsDataAccessHelper.AllInPages(PageNumber, RowsPerPage, "SP_GetAllGroupsInPages");

        public static DataTable AllStudentsInGroup(int? groupID)
            => clsDataAccessHelper.All("SP_GetAllStudentsInGroup", "GroupID", groupID);

        public static string GetGroupName(int? groupID)
        {
            string groupName = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SP_GetGroupName", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@GroupID", groupID);

                        SqlParameter outputIdParam = new SqlParameter("@GroupName", SqlDbType.NVarChar, 50)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputIdParam);

                        command.ExecuteNonQuery();

                        groupName = outputIdParam.Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                clsDataAccessHelper.HandleException(ex);
            }

            return groupName;
        }

        public static byte GetMaxCapacityOfStudentsInGroup(int? groupID)
        {
            byte maxCapacity = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SP_GetMaxCapacityOfStudentsInGroup", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@GroupID", groupID);

                        SqlParameter outputIdParam = new SqlParameter("@MaxCapacity", SqlDbType.TinyInt)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputIdParam);

                        command.ExecuteNonQuery();

                        maxCapacity = Convert.ToByte(outputIdParam.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                clsDataAccessHelper.HandleException(ex);
            }

            return maxCapacity;
        }

        public static DataTable AllGroupsAreTaughtByTeacher(int? teacherID)
            => clsDataAccessHelper.All("SP_GroupsAreTaughtByTeacher", "TeacherID", teacherID);

        public static DataTable AllGroupNames()
            => clsDataAccessHelper.All("SP_GetAllGroupNames");

        public static DataTable AllScheduleForToday()
            => clsDataAccessHelper.All("SP_GetScheduleForToday");

        public static int Count()
            => clsDataAccessHelper.Count("SP_GetAllGroupsCount");

        public static decimal GetSubjectFeesByGroupID(int? groupID)
        {
            decimal fees = 0m;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SP_GetSubjectFeesByGroupID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@GroupID", groupID);

                        SqlParameter outputIdParam = new SqlParameter("@Fees", SqlDbType.SmallMoney)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputIdParam);

                        command.ExecuteNonQuery();

                        fees = (decimal)outputIdParam.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                clsDataAccessHelper.HandleException(ex);
            }

            return fees;
        }
    }
}