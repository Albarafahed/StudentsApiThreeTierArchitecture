using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
namespace StudentDataAccessLayer
{
    public class StudentData
    {
        static string _connectionString = "Server=localhost\\SQLEXPRESS;Database=StudentsDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public static List<StudentDTO> GetAllStudents()
        {
            var StudentsList = new List<StudentDTO>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAllStudents", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StudentsList.Add(new StudentDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetInt32(reader.GetOrdinal("Age")),
                                reader.GetInt32(reader.GetOrdinal("Grade")),
                              reader.IsDBNull(reader.GetOrdinal("Email"))
    ?                       string.Empty
                            : reader.GetString(reader.GetOrdinal("Email")),

                        reader.IsDBNull(reader.GetOrdinal("PasswordHash"))
                            ? string.Empty
                            : reader.GetString(reader.GetOrdinal("PasswordHash")),

                        reader.IsDBNull(reader.GetOrdinal("Role"))
                            ? string.Empty
                            : reader.GetString(reader.GetOrdinal("Role"))
                                                    ));
                        }
                    }
                }


                return StudentsList;
            }

        }

        public static List<StudentDTO> GetPassedStudents()
        {
            var StudentsList = new List<StudentDTO>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetPassedStudents", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            StudentsList.Add(new StudentDTO
                            (
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetInt32(reader.GetOrdinal("Age")),
                                reader.GetInt32(reader.GetOrdinal("Grade")),
                                reader.GetString(reader.GetOrdinal("Email")),
                                reader.GetString(reader.GetOrdinal("PasswordHash")),
                                reader.GetString(reader.GetOrdinal("Role"))
                            ));
                        }
                    }
                }


                return StudentsList;
            }

        }

        public static double GetAverageGrade()
        {
            double averageGrade = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAverageGrade", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        averageGrade = Convert.ToDouble(result);
                    }
                    else
                        averageGrade = 0;

                }
            }

            return averageGrade;
        }

        public static StudentDTO? GetStudentById(int studentId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("SP_GetStudentById", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@StudentId", studentId);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new StudentDTO
                       (
                           reader.GetInt32(reader.GetOrdinal("Id")),
                           reader.GetString(reader.GetOrdinal("Name")),
                           reader.GetInt32(reader.GetOrdinal("Age")),
                           reader.GetInt32(reader.GetOrdinal("Grade")),
                          reader.GetString(reader.GetOrdinal("Email")),
                            reader.GetString(reader.GetOrdinal("PasswordHash")),
                             reader.GetString(reader.GetOrdinal("Role"))
                       );
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

      
        public static int AddStudent(StudentDTO StudentDTO)
        {
            using (var connection = new SqlConnection(_connectionString))
               
            using (var command = new SqlCommand("SP_AddStudent", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Name", StudentDTO.Name);
                command.Parameters.AddWithValue("@Age", StudentDTO.Age);
                command.Parameters.AddWithValue("@Grade", StudentDTO.Grade);
                command.Parameters.AddWithValue("@Email", StudentDTO.Email);
                command.Parameters.AddWithValue("@PasswordHash", StudentDTO.PasswordHash);
                command.Parameters.AddWithValue("@Role", StudentDTO.Role);
                var outputIdParam = new SqlParameter("@NewStudentId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(outputIdParam);

                connection.Open();
                command.ExecuteNonQuery();

                return (int)outputIdParam.Value;
            }
        }

        public static bool UpdateStudent(StudentDTO StudentDTO)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("SP_UpdateStudent", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@StudentId", StudentDTO.Id);
                command.Parameters.AddWithValue("@Name", StudentDTO.Name);
                command.Parameters.AddWithValue("@Age", StudentDTO.Age);
                command.Parameters.AddWithValue("@Grade", StudentDTO.Grade);
                command.Parameters.AddWithValue("@Email", StudentDTO.Email);
                command.Parameters.AddWithValue("@PasswordHash", StudentDTO.PasswordHash);
                command.Parameters.AddWithValue("@Role", StudentDTO.Role);

                connection.Open();
                command.ExecuteNonQuery();
                return true;

            }
        }

        public static bool DeleteStudent(int studentId)
        {

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("SP_DeleteStudent", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@StudentId", studentId);

                connection.Open();

                int rowsAffected = (int)command.ExecuteScalar();
                return (rowsAffected == 1);


            }
        }

        public static StudentLoginDTO? Login(string email)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("SP_Login", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Email", email);
              
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new StudentLoginDTO
                        (
                            reader.GetInt32(reader.GetOrdinal("Id")),
                            reader.GetString(reader.GetOrdinal("Name")),
                            reader.GetInt32(reader.GetOrdinal("Age")),
                            reader.GetInt32(reader.GetOrdinal("Grade")),
                          reader.GetString(reader.GetOrdinal("Email")),
                            reader.GetString(reader.GetOrdinal("PasswordHash")),
                             reader.GetString(reader.GetOrdinal("Role")),
                             reader.IsDBNull(reader.GetOrdinal("RefreshTokenHash")) ? null : reader.GetString(reader.GetOrdinal("RefreshTokenHash")),
                                reader.IsDBNull(reader.GetOrdinal("RefreshTokenExpiresAt")) ? null : reader.GetDateTime(reader.GetOrdinal("RefreshTokenExpiresAt")),
                                reader.IsDBNull(reader.GetOrdinal("RefreshTokenRevokedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("RefreshTokenRevokedAt"))
                        );
                    }
                    else
                    {
                        return null;
                    }   
                }
            }
        }

        public static bool UpdateRefreshToken(int studentId, string refreshTokenHash, DateTime? expiresAt)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("SP_UpdateRefreshToken", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@StudentId", studentId);
                command.Parameters.AddWithValue("@RefreshTokenHash", refreshTokenHash);
                command.Parameters.AddWithValue("@ExpiresAt", expiresAt);
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
    }
}
